using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Chroma.Graphics;
using Chroma.Input.GameControllers;
using Chroma;
using Chroma.Graphics.TextRendering.TrueType;
using Chroma.Windowing;

namespace GameController.Views
{
    public class GenericControllerView
    {
        private Queue<ControllerDriver> _scheduledForConnection = new();
        private Queue<ControllerDriver> _scheduledForRemoval = new();

        protected Window _window;
        protected RenderTarget _renderTarget;
        
        protected Dictionary<ControllerDriver, PlayerView> _controllers = new();

        protected float _movementSpeed = 300.0f;

        public virtual string ViewName => "Generic controllers";
        
        public virtual Color RightStickHatColor { get; } = Color.HotPink;

        public virtual List<ControllerType> AcceptedControllers { get; } = new()
        {
            ControllerType.Unknown,
            ControllerType.Virtual,
            ControllerType.Xbox360,
            ControllerType.XboxOne,
            ControllerType.AmazonLuna,
            ControllerType.GoogleStadia
        };

        public virtual Dictionary<ControllerButton, Color> ButtonColors { get; } = new()
        {
            { ControllerButton.A, Color.Lime },
            { ControllerButton.B, Color.Red },
            { ControllerButton.X, Color.Blue },
            { ControllerButton.Y, Color.Yellow }
        };

        public virtual Vector2 PositionOnScreen => new(
            _window.Width - _renderTarget.Width,
            _window.Height - _renderTarget.Height
        );

        public GenericControllerView(Window window)
        {
            _window = window;
            _renderTarget = new RenderTarget(window.Size / 2);
        }
        
        public virtual void OnConnected(ControllerEventArgs e)
        {
            if (!AcceptedControllers.Contains(e.Controller.Info.Type))
                return;
            
            e.Controller.SetDeadZone(ControllerAxis.LeftStickX, 4000);
            e.Controller.SetDeadZone(ControllerAxis.LeftStickY, 4000);

            _scheduledForConnection.Enqueue(e.Controller);
        }

        public virtual void OnDisconnected(ControllerEventArgs e)
        {
            if (!_controllers.ContainsKey(e.Controller))
                return;
            
            _scheduledForRemoval.Enqueue(e.Controller);
        }
        
        public virtual void OnTouchpadMoved(ControllerTouchpadEventArgs e)
        {
        }

        public virtual void OnTouchpadTouched(ControllerTouchpadEventArgs e)
        {
        }

        public virtual void OnTouchpadReleased(ControllerTouchpadEventArgs e)
        {
        }

        public virtual void OnSensorStateChanged(ControllerSensorEventArgs e)
        {
        }

        public virtual void OnAxisMoved(ControllerAxisEventArgs e)
        {
        }

        public virtual void OnButtonPressed(ControllerButtonEventArgs e)
        {
        }

        public virtual void OnButtonReleased(ControllerButtonEventArgs e)
        {
        }

        public virtual void Update(float delta)
        {
            while (_scheduledForConnection.Any())
            {
                var controller = _scheduledForConnection.Dequeue();
                
                _controllers.Add(
                    controller,
                    new PlayerView(
                        _renderTarget.Width / 2 - 16, 
                        _renderTarget.Height / 2 - 16, 
                        32, 
                        32
                    )
                );
            }

            foreach (var kvp in _controllers)
            {
                var playerView = kvp.Value;
                
                playerView.Rectangle.X += kvp.Key.GetAxisValueNormalized(ControllerAxis.LeftStickX) * delta * _movementSpeed;
                playerView.Rectangle.Y += kvp.Key.GetAxisValueNormalized(ControllerAxis.LeftStickY) * delta * _movementSpeed;

                if (playerView.Rectangle.X < 0)
                    playerView.Rectangle.X = 0;
                else if (playerView.Rectangle.X + playerView.Rectangle.Width > _renderTarget.Width)
                    playerView.Rectangle.X = _renderTarget.Width - playerView.Rectangle.Width;

                if (playerView.Rectangle.Y < 0)
                    playerView.Rectangle.Y = 0;
                else if (playerView.Rectangle.Y + playerView.Rectangle.Height > _renderTarget.Height)
                    playerView.Rectangle.Y = _renderTarget.Height - playerView.Rectangle.Height;
                
                playerView.TriggerLeft = kvp.Key.GetAxisValueNormalized(ControllerAxis.LeftTrigger);
                playerView.TriggerRight = kvp.Key.GetAxisValueNormalized(ControllerAxis.RightTrigger);
                playerView.RightStickX = kvp.Key.GetAxisValueNormalized(ControllerAxis.RightStickX);
                playerView.RightStickY = kvp.Key.GetAxisValueNormalized(ControllerAxis.RightStickY);
                
                _controllers[kvp.Key] = playerView;
                
                if (kvp.Key.IsButtonDown(ControllerButton.A))
                    playerView.Color = ButtonColors[ControllerButton.A];
                
                if (kvp.Key.IsButtonDown(ControllerButton.B))
                    playerView.Color = ButtonColors[ControllerButton.B];
                
                if (kvp.Key.IsButtonDown(ControllerButton.X))
                    playerView.Color = ButtonColors[ControllerButton.X];
                
                if (kvp.Key.IsButtonDown(ControllerButton.Y))
                    playerView.Color = ButtonColors[ControllerButton.Y];

                kvp.Key.SetLedColor(playerView.Color);
            }

            while (_scheduledForRemoval.Any())
            {
                var controller = _scheduledForRemoval.Dequeue();
                _controllers.Remove(controller);
            }
        }

        public virtual void Draw(RenderContext context)
        {
            context.RenderTo(_renderTarget, (ctx, tgt) =>
            {
                ctx.Clear(Color.Black);
                
                foreach (var kvp in _controllers)
                {
                    var controller = kvp.Key;
                    var player = kvp.Value;
                    ctx.Rectangle(ShapeMode.Fill, player.Rectangle, player.Color);
                    
                    var playerLabel = controller.Info.PlayerIndex.ToString();
                    var labelMeasure = TrueTypeFont.Default.Measure(playerLabel);

                    ctx.DrawString(
                        playerLabel,
                        new Vector2(
                            player.Rectangle.X + (player.Rectangle.Width / 2f - labelMeasure.Width / 2f),
                            player.Rectangle.Y + (player.Rectangle.Height / 2f - labelMeasure.Height / 2f)
                        ),
                        Color.Black
                    );

                    var playerButtonsLabel = string.Join(", ", controller.ActiveButtons);
                    var playerButtonsMeasure = TrueTypeFont.Default.Measure(playerButtonsLabel);
                    
                    ctx.DrawString(
                        playerButtonsLabel,
                        new Vector2(
                            player.Rectangle.X + (player.Rectangle.Width / 2f - playerButtonsMeasure.Width / 2f),
                            player.Rectangle.Y - TrueTypeFont.Default.Height - 8
                        )
                    );

                    if (player.TriggerLeft > 0)
                    {
                        var triggerLabel = player.TriggerLeft.ToString("F3");
                        var triggerMeasure = TrueTypeFont.Default.Measure(triggerLabel);
                        
                        ctx.DrawString(
                            triggerLabel,
                            new Vector2(
                                player.Rectangle.X - triggerMeasure.Width - 8,
                                player.Rectangle.Y + (player.Rectangle.Height / 2f - triggerMeasure.Height / 2f)
                            )
                        );

                        BlendTriggerBar(() =>
                        {
                            ctx.Rectangle(
                                ShapeMode.Fill,
                                player.Rectangle.X, 
                                player.Rectangle.Y + player.Rectangle.Height / 2 - 8,
                                -((triggerMeasure.Width + 16) * player.TriggerLeft),
                                16,
                                player.Color
                            );
                        });
                    }

                    if (player.TriggerRight > 0)
                    {
                        var triggerLabel = player.TriggerRight.ToString("F3");
                        var triggerMeasure = TrueTypeFont.Default.Measure(triggerLabel);
                        
                        ctx.DrawString(
                            triggerLabel,
                            new Vector2(
                                player.Rectangle.X + player.Rectangle.Width + 8,
                                player.Rectangle.Y + (player.Rectangle.Height / 2f - triggerMeasure.Height / 2f)
                            )
                        );
                        
                        BlendTriggerBar(() =>
                        {
                            ctx.Rectangle(
                                ShapeMode.Fill,
                                player.Rectangle.X + player.Rectangle.Width, 
                                player.Rectangle.Y + player.Rectangle.Height / 2 - 8,
                                (triggerMeasure.Width + 16) * player.TriggerRight,
                                16,
                                player.Color
                            );
                        });
                    }

                    var typeLabel = controller.Info.Type.ToString();
                    var typeMeasure = TrueTypeFont.Default.Measure(typeLabel);
                    ctx.DrawString(
                        typeLabel,
                        new Vector2(
                            player.Rectangle.X + (player.Rectangle.Width / 2f - typeMeasure.Width / 2f),
                            player.Rectangle.Y + player.Rectangle.Height + 4
                        ),
                        player.Color
                    );
                    
                    if (Math.Abs(player.RightStickX) > 0.09 || Math.Abs(player.RightStickY) > 0.09)
                    {
                        ctx.Circle(
                            ShapeMode.Fill, 
                            new Vector2(
                                player.Rectangle.X + player.Rectangle.Width / 2 + (32 * player.RightStickX),
                                player.Rectangle.Y + player.Rectangle.Height / 2  + (32 * player.RightStickY)
                            ),
                            6,
                            RightStickHatColor
                        );
                    }

                    DrawViewSpecific(controller, player, ctx);
                }

                PostDraw(context);
            
                context.DrawString(
                    ViewName, 
                    new(8, 8), 
                    Color.White
                );
            });
            
            context.DrawTexture(
                _renderTarget, 
                PositionOnScreen
            );
        }

        protected virtual void DrawViewSpecific(ControllerDriver controller, PlayerView player, RenderContext context)
        {
        }

        protected virtual void PostDraw(RenderContext context)
        {

        }
        
        private void BlendTriggerBar(Action renderLogic)
        {
            if (renderLogic == null)
                return;
            
            RenderSettings.SetShapeBlendingEquations(
                BlendingEquation.Subtract, 
                BlendingEquation.Add
            );
            RenderSettings.SetShapeBlendingFunctions(
                BlendingFunction.SourceColor,
                BlendingFunction.One,
                BlendingFunction.One,
                BlendingFunction.Zero
            );
            
            renderLogic();
            
            RenderSettings.ResetShapeBlending();
        }
    }
}