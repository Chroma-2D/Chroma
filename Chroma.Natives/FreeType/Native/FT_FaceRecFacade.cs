﻿using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Chroma.Natives.FreeType.Native;

namespace Chroma.Natives.FreeType.Native
{
    /// <summary>
    /// Represents an interface-agnostic facade over a FreeType2 face pointer.
    /// </summary>
    [Obsolete("FT_FaceRecFacade is deprecated, please use FreeTypeFaceFacade instead.", false)]
    public unsafe struct FT_FaceRecFacade
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FT_FaceRecFacade"/> structure.
        /// </summary>
        /// <param name="face">A pointer to the wrapped FreeType2 face object.</param>
        public FT_FaceRecFacade(IntPtr face, IntPtr library)
        {
            _Face = face;
            _FaceRec = (FT_FaceRec*)this._Face;
            _Library = library;
        }

        /// <summary>
        /// Selects the specified character size for the font face.
        /// </summary>
        /// <param name="sizeInPoints">The size in points to select.</param>
        /// <param name="dpiX">The horizontal pixel density.</param>
        /// <param name="dpiY">The vertical pixel density.</param>
        public void SelectCharSize(int sizeInPoints, uint dpiX, uint dpiY)
        {
            var size = (IntPtr)(sizeInPoints << 6);
            var err = FT.FT_Set_Char_Size(_Face, size, size, dpiX, dpiY);
            if (err != FT_Error.FT_Err_Ok)
                throw new FreeTypeException(err);
        }

        /// <summary>
        /// Selects the specified fixed size for the font face.
        /// </summary>
        /// <param name="ix">The index of the fixed size to select.</param>
        public void SelectFixedSize(int ix)
        {
            var err = FT.FT_Select_Size(_Face, ix);
            if (err != FT_Error.FT_Err_Ok)
                throw new FreeTypeException(err);
        }

        /// <summary>
        /// Gets the glyph index of the specified character, if it is defined by this face.
        /// </summary>
        /// <param name="charcode">The character code for which to retrieve a glyph index.</param>
        /// <returns>The glyph index of the specified character, or 0 if the character is not defined by this face.</returns>
        public uint GetCharIndex(uint charCode) { return FT.FT_Get_Char_Index(_Face, charCode); }

        /// <summary>
        /// Marshals the face's family name to a C# string.
        /// </summary>
        /// <returns>The marshalled string.</returns>
        public string MarshalFamilyName() { return Marshal.PtrToStringAnsi(_FaceRec->family_name); }

        /// <summary>
        /// Marshals the face's style name to a C# string.
        /// </summary>
        /// <returns>The marshalled string.</returns>
        public string MarshalStyleName() { return Marshal.PtrToStringAnsi(_FaceRec->style_name); }

        /// <summary>
        /// Returns the specified character if it is defined by this face; otherwise, returns <see langword="null"/>.
        /// </summary>
        /// <param name="c">The character to evaluate.</param>
        /// <returns>The specified character, if it is defined by this face; otherwise, <see langword="null"/>.</returns>
        public char? GetCharIfDefined(Char c) { return FT.FT_Get_Char_Index(_Face, c) > 0 ? c : (char?)null; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetFixedSizeInPixels(FT_FaceRec* face, int ix)
        {
            return face->available_sizes[ix].height;
        }

        /// <summary>
        /// Returns the index of the fixed size which is the closest match to the specified pixel size.
        /// </summary>
        /// <param name="sizeInPixels">The desired size in pixels.</param>
        /// <param name="requireExactMatch">A value indicating whether to require an exact match.</param>
        /// <returns>The index of the closest matching fixed size.</returns>
        public int FindNearestMatchingPixelSize(int sizeInPixels, bool requireExactMatch = false)
        {
            var numFixedSizes = _FaceRec->num_fixed_sizes;
            if (numFixedSizes == 0)
                throw new InvalidOperationException("FONT_DOES_NOT_HAVE_BITMAP_STRIKES");

            var bestMatchIx = 0;
            var bestMatchDiff = Math.Abs(GetFixedSizeInPixels(_FaceRec, 0) - sizeInPixels);

            for (int i = 0; i < numFixedSizes; i++)
            {
                var size = GetFixedSizeInPixels(_FaceRec, i);
                var diff = Math.Abs(size - sizeInPixels);
                if (diff < bestMatchDiff)
                {
                    bestMatchDiff = diff;
                    bestMatchIx = i;
                }
            }

            if (bestMatchDiff != 0 && requireExactMatch)
                throw new InvalidOperationException(string.Format("NO_MATCHING_PIXEL_SIZE: {0}", sizeInPixels));

            return bestMatchIx;
        }

        public bool EmboldenGlyphBitmap(int xStrength, int yStrength)
        {
            var err = FT.FT_Bitmap_Embolden(_Library, (IntPtr)(GlyphBitmapPtr), (IntPtr)xStrength, (IntPtr)yStrength);
            if (err != FT_Error.FT_Err_Ok)
                return false;

            if ((int)_FaceRec->glyph->advance.x > 0)
                _FaceRec->glyph->advance.x += xStrength;
            if ((int)_FaceRec->glyph->advance.y > 0)
                _FaceRec->glyph->advance.x += yStrength;
            _FaceRec->glyph->metrics.width += xStrength;
            _FaceRec->glyph->metrics.height += yStrength;
            _FaceRec->glyph->metrics.horiBearingY += yStrength;
            _FaceRec->glyph->metrics.horiAdvance += xStrength;
            _FaceRec->glyph->metrics.vertBearingX -= xStrength / 2;
            _FaceRec->glyph->metrics.vertBearingY += yStrength;
            _FaceRec->glyph->metrics.vertAdvance += yStrength;

            _FaceRec->glyph->bitmap_top += (yStrength >> 6);

            return true;
        }

        /// <summary>
        /// Gets a value indicating whether the face has the specified flag defined.
        /// </summary>
        /// <param name="flag">The flag to evaluate.</param>
        /// <returns><see langword="true"/> if the face has the specified flag defined; otherwise, <see langword="false"/>.</returns>
        public bool HasFaceFlag(int flag) { return (((int)_FaceRec->face_flags) & flag) != 0; }

        /// <summary>
        /// Gets a value indicating whether the face has the FT_FACE_FLAG_SCALABLE flag set.
        /// </summary>
        /// <returns><see langword="true"/> if the face has the FT_FACE_FLAG_SCALABLE flag defined; otherwise, <see langword="false"/>.</returns>
        public bool HasScalableFlag { get { return HasFaceFlag(FT.FT_FACE_FLAG_SCALABLE); } }

        /// <summary>
        /// Gets a value indicating whether the face has the FT_FACE_FLAG_FIXED_SIZES flag set.
        /// </summary>
        /// <returns><see langword="true"/> if the face has the FT_FACE_FLAG_FIXED_SIZES flag defined; otherwise, <see langword="false"/>.</returns>
        public bool HasFixedSizes { get { return HasFaceFlag(FT.FT_FACE_FLAG_FIXED_SIZES); } }

        /// <summary>
        /// Gets a value indicating whether the face has the FT_FACE_FLAG_COLOR flag set.
        /// </summary>
        /// <returns><see langword="true"/> if the face has the FT_FACE_FLAG_COLOR flag defined; otherwise, <see langword="false"/>.</returns>
        public bool HasColorFlag { get { return HasFaceFlag(FT.FT_FACE_FLAG_COLOR); } }

        /// <summary>
        /// Gets a value indicating whether the face has the FT_FACE_FLAG_KERNING flag set.
        /// </summary>
        /// <returns><see langword="true"/> if the face has the FT_FACE_FLAG_KERNING flag defined; otherwise, <see langword="false"/>.</returns>
        public bool HasKerningFlag { get { return HasFaceFlag(FT.FT_FACE_FLAG_KERNING); } }

        /// <summary>
        /// Gets a value indicating whether the face has any bitmap strikes with fixed sizes.
        /// </summary>
        public bool HasBitmapStrikes { get { return (_FaceRec->num_fixed_sizes) > 0; } }

        /// <summary>
        /// Gets the current glyph bitmap.
        /// </summary>
        public FT_Bitmap GlyphBitmap { get { return _FaceRec->glyph->bitmap; } }
        public FT_Bitmap* GlyphBitmapPtr { get { return &_FaceRec->glyph->bitmap; } }

        /// <summary>
        /// Gets the left offset of the current glyph bitmap.
        /// </summary>
        public int GlyphBitmapLeft { get { return _FaceRec->glyph->bitmap_left; } }

        /// <summary>
        /// Gets the right offset of the current glyph bitmap.
        /// </summary>
        public int GlyphBitmapTop { get { return _FaceRec->glyph->bitmap_top; } }

        /// <summary>
        /// Gets the width in pixels of the current glyph.
        /// </summary>
        public int GlyphMetricWidth { get { return (int)_FaceRec->glyph->metrics.width >> 6; } }

        /// <summary>
        /// Gets the height in pixels of the current glyph.
        /// </summary>
        public int GlyphMetricHeight { get { return (int)_FaceRec->glyph->metrics.height >> 6; } }

        /// <summary>
        /// Gets the horizontal advance of the current glyph.
        /// </summary>
        public int GlyphMetricHorizontalAdvance { get { return (int)_FaceRec->glyph->metrics.horiAdvance >> 6; } }

        /// <summary>
        /// Gets the vertical advance of the current glyph.
        /// </summary>
        public int GlyphMetricVerticalAdvance { get { return (int)_FaceRec->glyph->metrics.vertAdvance >> 6; } }

        /// <summary>
        /// Gets the face's ascender size in pixels.
        /// </summary>
        public int Ascender { get { return (int)_FaceRec->size->metrics.ascender >> 6; } }

        /// <summary>
        /// Gets the face's descender size in pixels.
        /// </summary>
        public int Descender { get { return (int)_FaceRec->size->metrics.descender >> 6; } }

        /// <summary>
        /// Gets the face's line spacing in pixels.
        /// </summary>
        public int LineSpacing { get { return (int)_FaceRec->size->metrics.height >> 6; } }

        /// <summary>
        /// Gets the face's underline position.
        /// </summary>
        public int UnderlinePosition { get { return _FaceRec->underline_position >> 6; } }

        /// <summary>
        /// Gets a pointer to the face's glyph slot.
        /// </summary>
        public FT_GlyphSlotRec* GlyphSlot { get { return _FaceRec->glyph; } }

        // A pointer to the wrapped FreeType2 face object.
        private readonly IntPtr _Face;
        private readonly FT_FaceRec* _FaceRec;
        private readonly IntPtr _Library;
    }
}
