﻿/*
	Include in output:

	This file is part of Natural Docs, which is Copyright © 2003-2022 Code Clear LLC.
	Natural Docs is licensed under version 3 of the GNU Affero General Public
	License (AGPL).  Refer to License.txt or www.naturaldocs.org for the
	complete details.

	This file may be distributed with documentation files generated by Natural Docs.
	Such documentation is not covered by Natural Docs' copyright and licensing,
	and may have its own copyright and distribution terms as decided by its author.

*/

"use strict";


// Location Info Members

	$LocationInfo_SimpleIdentifier = 0;
	$LocationInfo_Folder = 1;
	$LocationInfo_Type = 2;
	$LocationInfo_PrefixRegexString = 3;
	$LocationInfo_PrefixRegexObject = 4;

// Location Info Type values

	$LocationInfoType_File = 0
	$LocationInfoType_LanguageSpecificHierarchy = 1;
	$LocationInfoType_LanguageAgnosticHierarchy = 2;



/* Class: NDFramePage
	_____________________________________________________________________________

*/
var NDFramePage = new function ()
	{

	// Group: Functions
	// ________________________________________________________________________


	/* Function: Start
	*/
	this.Start = function ()
		{

		// The default title of the page is the project title.  Save a copy before we mess with it.

		this.projectTitle = document.title;


		// Transition the page from our static default loading screen to the active layout.

		var loadingNotice = document.getElementById("NDLoadingNotice");
		loadingNotice.parentNode.removeChild(loadingNotice);

		// True or false determines whether it will be made visible.
		var pageElements = {
			NDHeader: true,
			NDSearchField: true,
			NDFooter: true,
			NDMenu: true,
			NDSummary: false, // UpdateLayout() will enable this if necessary
			NDContent: true,
			NDMenuSizer: true, // Needs to be visible, but is styled as transparent unless hovered over
			NDSummarySizer: true // Needs to be visible, but is styled as transparent unless hovered over
			};

		// Update the layout.
		for (var pageElementName in pageElements)
			{
			var domElement = document.getElementById(pageElementName);
			domElement.style.position = "fixed";

			if (pageElements[pageElementName] == true)
				{  domElement.style.display = "block";  }
			}

		// this.desiredSearchWidth = undefined;
		// this.desiredMenuWidth = undefined;
		// this.desiredSummaryWidth = undefined;

		this.UpdateLayout();


		// Attach event handlers.  All browsers Natural Docs supports support onhashchange now, including IE 11 and
		// EdgeHTML.

		window.onresize = function () {  NDFramePage.OnResize();  };
		// window.onhashchange = function () {  NDFramePage.OnHashChange();  };  // Wait until OnLocationsLoaded
		document.onmousedown = function (e) {  return NDFramePage.OnMouseDown(e);  };


		// ...however, IE 11 and EdgeHTML still use case-insensitive anchors, so we still need to poll the location to detect
		// navigation between "Member" and "member".  They'll still mess up the back/forward browser history a bit, but
		// at least clicking between them will work.

		if (NDCore.CaseInsensitiveAnchors())
			{  this.CreateHashChangePoller();  }


		// We want to close the search results when we click anywhere else.  OnMouseDown handles clicks on most of the
		// page but not inside the content iframe.  window.onblur will fire for IE 10, Firefox, and Chrome.  document.onblur
		// will only fire for Firefox.  IE 8 and earlier won't fire either, oh well.
		window.onblur = function () {  NDFramePage.OnBlur();  };


		// Start panels

		NDMenu.Start();
		NDSummary.Start();
		NDSearch.Start();


		// We have to wait for OnLocationsLoaded() to interpret the hash path, so we don't call OnHashChange() here.  It
		// will be done by OnLocationsLoaded() instead.

		};


	/* Function: OnBlur
	*/
	this.OnBlur = function ()
		{
		if (NDSearch.SearchFieldIsActive())
			{
			NDSearch.ClearResults();
			NDSearch.DeactivateSearchField();
			}
		};



	// Group: Hash and Navigation Functions
	// ________________________________________________________________________


	/* Function: OnHashChange
	*/
	this.OnHashChange = function ()
		{
		var oldLocation = this.currentLocation;
		this.currentLocation = new NDLocation(location.hash);


		// Update the poller for the browsers that need it to prevent this function from being called twice.  The poller will
		// therefore only catch navigation that the event doesn't fire for, like moving from "Member" to "member".

		if (this.hashChangePoller != undefined)
			{  this.hashChangePoller.lastHash = location.hash;  }


		// Clear any search results since that may be the way we got here.

		NDSearch.ClearResults();
		NDSearch.DeactivateSearchField();


		// If we're using a source file for the home page, substitute its location.  We don't have to worry about handling
		// custom home pages based on HTML files here because they get resaved into the default location.

		if (this.currentLocation.type == "Home" && this.sourceFileHomePageHashPath)
			{
			var homePageLocation = new NDLocation(this.sourceFileHomePageHashPath);
			homePageLocation.type = "Home";

			this.currentLocation = homePageLocation;
			}


		// We need to update the layout if the location changes the visibility of the summary panel.

		var oldLocationHasSummary = (oldLocation != undefined && oldLocation.summaryFile != undefined);
		var currentLocationHasSummary = (this.currentLocation.summaryFile != undefined);

		if (oldLocationHasSummary != currentLocationHasSummary)
			{  this.UpdateLayout();  }


		// Set the content page

		var frame = document.getElementById("CFrame");

		// If the browser treats anchors as case-insensitive and the hash path has a member we won't do the navigation here because
		// it would confuse member and Member.  Instead we'll let the summary look up the member and do the navigation with a
		// Topic# anchor.
		if (NDCore.CaseInsensitiveAnchors() && this.currentLocation.member != undefined)
			{
			// Do nothing.  We can't even go to the base page here and let the summary replace it with the anchor because they don't
			// always occur in the right order.
			}
		else
			{
			// Everything else is case sensitive and can go right to the target without waiting for the summary to load.
			frame.contentWindow.location.replace(this.currentLocation.contentPage);
			}


		// Set focus to the content page iframe so that keyboard scrolling works without clicking over to it.

		frame.contentWindow.focus();


		// Notify the panels

		NDMenu.OnLocationChange(oldLocation, this.currentLocation);
		NDSummary.OnLocationChange(oldLocation, this.currentLocation);


		// Normally the page title will be updated by the summary metadata file, but we have to do it manually if the new
		// location won't have a summary, such as the home page.

		if (this.currentLocation.summaryFile == undefined)
			{  this.UpdatePageTitle();  }
		};


	/* Function: OnLocationsLoaded
	*/
	this.OnLocationsLoaded = function (locationInfo, sourceFileHomePageHashPath)
		{
		this.locationInfo = locationInfo;

		// Create RegExp objects since they're not included in the data
		for (var i = 0; i < this.locationInfo.length; i++)
			{
			this.locationInfo[i][$LocationInfo_PrefixRegexObject] = new RegExp( this.locationInfo[i][$LocationInfo_PrefixRegexString] );
			}

		this.sourceFileHomePageHashPath = sourceFileHomePageHashPath;

		// Now we can interpret the initial hash path and set the event handler for future ones.
		window.onhashchange = function () {  NDFramePage.OnHashChange();  };
		this.OnHashChange();
		};


	/* Function: OnPageTitleLoaded
		Called by a source file's metadata when it's loaded.
	*/
	this.OnPageTitleLoaded = function (hashPath, title)
		{
		if (this.currentLocation.path == hashPath)
			{
			// If we're handling a source file being loaded as the home page, ignore its page title and set it back to the project's.
			if (this.currentLocation.type == "Home")
				{  this.UpdatePageTitle();  }
			else
				{  this.UpdatePageTitle(title);  }
			}
		};


	/* Function: UpdatePageTitle
		Changes the page title to one incorporating the passed source page title.  If the page title is undefined it gets set back to
		the default page title.
	*/
	this.UpdatePageTitle = function (pageTitle)
		{
		if (pageTitle)
			{  document.title = pageTitle + " - " + this.projectTitle;  }
		else
			{  document.title = this.projectTitle;  }
		};


	/* Function: CreateHashChangePoller
		Creates and starts <hashChangePoller> to make sure browsers with case-insensitive anchors still fire <OnHashChange()>
		when navigating between "Member" and "member".
	*/
	this.CreateHashChangePoller = function ()
		{
		this.hashChangePoller = {
			// timeoutID: undefined,
			timeoutLength: 200,  // Every fifth of a second

			// Remember the initial hash so it doesn't get triggered immediately.
			lastHash: location.hash
			};

		this.hashChangePoller.Start = function ()
			{
			this.Poll();
			};

		this.hashChangePoller.Stop = function ()
			{
			if (this.timeoutID != undefined)
				{
				clearTimeout(this.timeoutID);
				this.timeoutID = undefined;
				}
			};

		this.hashChangePoller.Poll = function ()
			{
			if (location.hash != this.lastHash)
				{
				this.lastHash = location.hash;
				NDFramePage.OnHashChange();
				}

			this.timeoutID = setTimeout("NDFramePage.hashChangePoller.Poll()", this.timeoutLength);
			};

		this.hashChangePoller.Start();
		};



	// Group: Layout Functions
	// ________________________________________________________________________


	/* Function: OnResize
	*/
	this.OnResize = function ()
		{
		this.UpdateLayout();
		};


	/* Function: UpdateLayout
		Positions all elements on the page.
	*/
	this.UpdateLayout = function ()
		{
		var fullWidth = window.innerWidth;
		var fullHeight = window.innerHeight;

		var header = document.getElementById("NDHeader");
		var searchField = document.getElementById("NDSearchField");
		var footer = document.getElementById("NDFooter");
		var menu = document.getElementById("NDMenu");
		var menuSizer = document.getElementById("NDMenuSizer");
		var summary = document.getElementById("NDSummary");
		var summarySizer = document.getElementById("NDSummarySizer");
		var content = document.getElementById("NDContent");


		// Header

		header.style.left = "0px";
		header.style.top = "0px";
		header.style.width = fullWidth + "px";


		// Treat the header as one pixel shorter than it actually is.  This makes it so it there's a lip that sits under the
		// rest of the page elements.  We do this because when browsers are set to zoom levels greater than 100%,
		// rounding errors may make 1px gaps appear between elements.  For most elements this isn't as issue because
		// the menu background color blends in.  The header is different because a gray bar between it and the home
		// page is very noticable.
		var headerHeight = header.offsetHeight - 1;

		// firstChild moves from the div to the link
		var headerTitle = document.getElementById("HTitle").firstChild;
		var headerTitleRightEdge = headerTitle.offsetLeft + headerTitle.offsetWidth;

		var headerSubTitle = document.getElementById("HSubtitle");  // may not exist
		var headerSubTitleRightEdge = 0;

		if (headerSubTitle)
			{
			headerSubTitle = headerSubTitle.firstChild;
			headerSubTitleRightEdge = headerSubTitle.offsetLeft + headerSubTitle.offsetWidth;
			}

		var headerRightEdge = Math.max(headerTitleRightEdge, headerSubTitleRightEdge);


		// Search field

		if (this.desiredSearchWidth == undefined)
			{  this.desiredSearchWidth = searchField.offsetWidth;  }

		var searchMargin = Math.floor((headerHeight - searchField.offsetHeight) / 2);

		var searchWidth = this.desiredSearchWidth;
		var maxSearchWidth = fullWidth - headerRightEdge - (searchMargin * 4);  // 3x left margin + right margin

		if (searchWidth > maxSearchWidth)
			{  searchWidth = maxSearchWidth;  }

		searchField.style.left = (fullWidth - searchWidth - searchMargin) + "px";
		searchField.style.top = searchMargin + "px";
		searchField.style.width = searchWidth + "px";


		// Menu and footer

		var remainingHeight = fullHeight - headerHeight;
		var remainingWidth = fullWidth;
		var currentX = 0;
		var currentY = headerHeight;

		// The order of operations below is very important.  Block has to be set before checking the offset width or it
		// might return zero.  It also has to be set before setting the position or Firefox will sometimes not show
		// scrollbars on the summary panel when navigating back and forth between the home page where it's hidden
		// and regular pages where it's not.

		menu.style.display = "block";
		menu.style.left = currentX + "px";
		menu.style.top = currentY + "px";

		// The menu's default width might be set in something other than pixels, like ex, which might make it fractional
		// when converted to pixels.  This can create black bars between panels in Firefox.  offsetWidth always returns
		// pixels so re-set the width to guarantee whole pixels.
		var menuWidth = menu.offsetWidth;
		menu.style.width = menuWidth + "px";

		footer.style.left = currentX + "px";
		footer.style.top = currentY + "px";
		footer.style.width = menuWidth + "px";
		var footerHeight = footer.offsetHeight;

		menu.style.height = (remainingHeight - footerHeight) + "px";
		footer.style.top = (currentY + remainingHeight - footerHeight) + "px";

		if (this.desiredMenuWidth == undefined)
			{  this.desiredMenuWidth = menuWidth;  }

		menuSizer.style.display = "block";
		menuSizer.style.left = currentX + menuWidth + "px";
		menuSizer.style.top = currentY + "px";
		menuSizer.style.height = remainingHeight + "px";

		NDMenu.OnUpdateLayout();

		currentX += menuWidth;
		remainingWidth -= menuWidth;


		// Summary

		if (this.SummaryIsVisible())
			{
			summary.style.display = "block";
			summary.style.left = currentX + "px";
			summary.style.top = currentY + "px";
			summary.style.height = remainingHeight + "px";

			// The summary's width might be set in something other than pixels, like ex, which might make it fractional when
			// converted to pixels.  This can create black bars between panels in Firefox.  offsetWidth always returns pixels
			// so re-set the width to guarantee whole pixels.
			var summaryWidth = summary.offsetWidth;
			summary.style.width = summaryWidth + "px";

			if (this.desiredSummaryWidth == undefined)
				{  this.desiredSummaryWidth = summaryWidth;  }

			summarySizer.style.display = "block";
			summarySizer.style.left = (currentX + summaryWidth - 1) + "px";
			summarySizer.style.top = currentY + "px";
			summarySizer.style.height = remainingHeight + "px";

			currentX += summaryWidth;
			remainingWidth -= summaryWidth;
			}
		else
			{
			summary.style.display = "none";
			summarySizer.style.display = "none";
			}

		content.style.left = currentX + "px";
		content.style.top = currentY + "px";
		content.style.width = remainingWidth + "px";
		content.style.height = remainingHeight + "px";

		NDSearch.OnUpdateLayout();
		};


	/* Function: SummaryIsVisible
	*/
	this.SummaryIsVisible = function ()
		{
		return (this.currentLocation != undefined && this.currentLocation.summaryFile != undefined);
		};


	/* Function: OnMouseDown
	*/
	this.OnMouseDown = function (event)
		{
		if (event == undefined)
			{  event = window.event;  }

		var target = event.target || event.srcElement;

		if (NDSearch.SearchFieldIsActive())
			{
			var targetIsPartOfSearch = false;

			for (var element = target; element != undefined; element = element.parentNode)
				{
				if (element.id == "NDSearchResults" ||
					element.id == "NDSearchField")
					{
					targetIsPartOfSearch = true;
					break;
					}
				}

			if (!targetIsPartOfSearch)
				{
				NDSearch.ClearResults();
				NDSearch.DeactivateSearchField();
				}
			}

		if (target.id == "NDMenuSizer" || target.id == "NDSummarySizer")
			{
			var panel;

			if (target.id == "NDMenuSizer")
				{  panel = document.getElementById("NDMenu");  }
			else
				{  panel = document.getElementById("NDSummary");  }

			this.sizerDragging =
				{
				"sizer": target,
				"panel": panel,
				"originalSizerX": target.offsetLeft,
				"originalPanelWidth": panel.offsetWidth,
				"originalClientX": event.clientX
				};

			NDCore.AddClass(target, "Dragging");

			document.onmousemove = function (e) {  return NDFramePage.OnSizerMouseMove(e);  };
			document.onmouseup = function (e) {  return NDFramePage.OnSizerMouseUp(e);  };
			document.onselectstart = function () {  return false;  };  // Helps IE

			// We need a div to cover the content iframe or else if you drag too fast over it IE will send some of the messages
			// to the iframe instead.  The z-index is set in CSS to be between the sizers and everything else.
			var contentCover = document.createElement("div");
			contentCover.id = "NDContentCover";

			contentCover.style.left = "0px";
			contentCover.style.top = "0px";
			contentCover.style.width = window.innerWidth + "px";
			contentCover.style.height = window.innerHeight + "px";

			document.body.appendChild(contentCover);

			return false;
			}

		// If we click a link in the summary, then scroll away, then click the same link, the view won't move back to the
		// topic that was clicked on because the URL didn't change and therefore there's no onhashchange event.  Since 
		// we already have an onmousedown handler we can check for this and manually call it.
		else if (target.tagName == "A" && 
				  target.href == window.location)
			{  
			this.OnHashChange();
			return false;
			}

		else
			{  return true;  }
		};


	/* Function: OnSizerMouseMove
	*/
	this.OnSizerMouseMove = function (event)
		{
		if (event == undefined)
			{  event = window.event;  }

		var offset = event.clientX - this.sizerDragging.originalClientX;
		var windowClientWidth = window.innerWidth;

		// Sanity checks
		if (this.sizerDragging.sizer.id == "NDMenuSizer")
			{
			if (this.sizerDragging.originalSizerX + offset < 0)
				{  offset = 0 - this.sizerDragging.originalSizerX;  }
			else if (this.sizerDragging.originalSizerX + offset + this.sizerDragging.sizer.offsetWidth > windowClientWidth)
				{  offset = windowClientWidth - this.sizerDragging.sizer.offsetWidth - this.sizerDragging.originalSizerX;  }
			}
		else // "NDSummarySizer"
			{
			var menuSizer = document.getElementById("NDMenuSizer");
			var leftLimit = menuSizer.offsetLeft + menuSizer.offsetWidth;

			if (this.sizerDragging.originalSizerX + offset < leftLimit)
				{  offset = leftLimit - this.sizerDragging.originalSizerX;  }
			else if (this.sizerDragging.originalSizerX + offset + this.sizerDragging.sizer.offsetWidth > windowClientWidth)
				{  offset = windowClientWidth - this.sizerDragging.sizer.offsetWidth - this.sizerDragging.originalSizerX;  }
			}

		this.sizerDragging.sizer.style.left = (this.sizerDragging.originalSizerX + offset) + "px";
		this.sizerDragging.panel.style.width = (this.sizerDragging.originalPanelWidth + offset) + "px";

		if (this.sizerDragging.sizer.id == "NDMenuSizer")
			{  this.desiredMenuWidth = document.getElementById("NDMenu").offsetWidth;  }
		else // "NDSummarySizer
			{  this.desiredSummaryWidth = document.getElementById("NDSummary").offsetWidth;  }

		this.UpdateLayout();
		};


	/* Function: OnSizerMouseUp
	*/
	this.OnSizerMouseUp = function (event)
		{
		// Doesn't work if you use undefined.  Must be null.
		document.onmousemove = null;
		document.onmouseup = null;
		document.onselectstart = null;

		document.body.removeChild(document.getElementById("NDContentCover"));

		NDCore.RemoveClass(this.sizerDragging.sizer, "Dragging");
		this.sizerDragging = undefined;
		};


	/* Function: SizeSummaryToContent
		Resizes the summary panel to try to show its content without a horizontal scrollbar.  The new width will have a
		minimum of <desiredSummaryWidth> and a maximum of <desiredSummaryWidth> times <$ExpansionFactor>.  This
		is to be called by <NDSummary> whenever it's content changes.
	*/
	this.SizeSummaryToContent = function ()
		{
		this.SizePanelToContent(document.getElementById("NDSummary"), this.desiredSummaryWidth);
		};


	// I decided not to implement similar functionality for NDMenu, though it can be supported just as easily.  Just create
	// SizeMenuToContent() and call it from NDMenu.Update().


	/* Function: SizePanelToContent
		Resizes the passed panel to try to show its content without a horizontal scrollbar.  The new width will have a
		minimum of desiredOffsetWidth and a maximum of desiredOffsetWidth times <$ExpansionFactor>.
	*/
	this.SizePanelToContent = function (panel, desiredOffsetWidth)
		{
		// For reference:
		//    clientWidth/Height - Size of visible content area
		//    offsetWidth/Height - Size of visible content area plus scrollbars and borders
		//    scrollWidth/Height - Size of content

		// This may happen the first time the panel is loaded if it happens before the first UpdateLayout().
		if (this.desiredSummaryWidth == undefined)
			{  return;  }

		var resized = false;

		// If there's no horizontal scroll bar...
		// (scrollWidth will never be less than clientWidth, even if the content doesn't need all the room.)
		if (panel.clientWidth == panel.scrollWidth)
			{
			// and we're already at the desired width, there's nothing to do.
			if (panel.offsetWidth == desiredOffsetWidth)
				{  return;  }
			else
				{
				// The panel is different than the desired width, meaning it was automatically expanded for the previous
				// content.  There's no way for us to determine the minimum content width to only shrink it down when
				// necessary, so we have to reset it and then determine if we need to expand it again.
				panel.style.width = desiredOffsetWidth + "px";
				resized = true;
				}
			}
		// else
			// If there is a horizontal scrollbar, that means scrollWidth is set to the minimum content width and we can
			// continue regardless of whether the panel is the desired size.

		var newOffsetWidth = panel.scrollWidth;

		// Do we have a vertical scroll bar?
		if (panel.scrollHeight > panel.clientHeight)
			{
			// If so factor it in.  offset - client will include the left and right border widths too.
			newOffsetWidth += panel.offsetWidth - panel.clientWidth;
			}
		else
			{
			// If not just factor in the border widths.  This only works if they're specified in px in the CSS.
			newOffsetWidth += NDCore.GetComputedPixelWidth(panel, "borderLeftWidth") +
										 NDCore.GetComputedPixelWidth(panel, "borderRightWidth");
			}

		// At this point newOffsetWidth is either the same as desiredOffsetWidth or is a larger value representing the
		// minimum content size.  Search your feelings, you know it to be true.  Or just work through all the possibilities
		// in the above code.  Whatever.

		if (newOffsetWidth != desiredOffsetWidth)
			{
			// Okay, so we're larger than the desired width.  Add a few pixels for padding.
			newOffsetWidth += 3;

			// See if automatically expanding to this size would exceed the maximum.
			if (newOffsetWidth / desiredOffsetWidth > $ExpansionFactor)
				{
				newOffsetWidth = Math.floor(desiredOffsetWidth * $ExpansionFactor);
				}

			if (panel.offsetWidth != newOffsetWidth)
				{
				panel.style.width = newOffsetWidth + "px";
				resized = true;
				}
			}

		if (resized)
			{  this.UpdateLayout();  }
		};



	// Group: Variables
	// ________________________________________________________________________

	/* var: currentLocation
		A <NDLocation> representing the current hash path location.
	*/

	/* var: locationInfo
		An array of location information objects as documented in <Location Information>, or undefined if <OnLocationsLoaded()> hasn't
		been called yet.
	*/

	/* var: projectTitle
		The project title in HTML.
	*/

	/* var: sourceFileHomePageHashPath
		The hash path of the source file serving as a custom home page, or undefined if none.  This will be undefined if there is no custom
		home page or if there is a custom home page but it is a HTML file.
	*/

	/* var: hashChangePoller
		An object to assist with hash change polling on browsers that use case-insensitive anchors.
	*/



	// Group: Layout Variables
	// ________________________________________________________________________

	/* var: sizerDragging

		If we're currently dragging a sizer, this will be an object with these members:

		sizer - The sizer DOM element.
		panel - The DOM element of the panel the sizer is stretching.
		originalSizerX - The sizer's original X position.
		originalPanelWidth - The panel's original width.
		originalClientX - The mouse's original X position.
	*/

	/* var: desiredSearchWidth
		The width the search field should use, or undefined to use the default.  The actual search field may be
		shorter if it is space constrained.
	*/

	/* var: desiredMenuWidth
		The width the menu panel should use, or undefined to use the default.  The actual menu width can be
		slightly larger if needed to show the content without a horizontal scrollbar.
	*/

	/* var: desiredSummaryWidth
		The width the summary panel should use, or undefined to use the default.  The actual summary width
		can be slightly larger if needed to show the content without a horizontal scrollbar.
	*/

	/* Constant: $ExpansionFactor
		This substitution is the maximum amount the menu or summary panel may be automatically expanded by.
		To allow a 15% expansion, set the value to 1.15.
	*/
	$ExpansionFactor = 1.333;

	};