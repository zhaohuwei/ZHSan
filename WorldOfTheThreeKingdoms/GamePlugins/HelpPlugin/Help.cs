﻿using GameFreeText;
using GameGlobal;
using GameManager;
using GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace HelpPlugin
{

    internal class Help
    {
        internal Point BackgroundSize;
        internal PlatformTexture BackgroundTexture;
        internal Point ButtonDisplayOffset;
        private PlatformTexture ButtonDisplayTexture;
        internal PlatformTexture ButtonSelectedTexture;
        internal Point ButtonSize;
        internal PlatformTexture ButtonTexture;
        internal GameObjectTextBranch CurrentBranch;
        internal Point DisplayOffset;
        private bool isButtonShowing;
        private bool isShowing;
        internal FreeRichText RichText = new FreeRichText();
        
        internal GameObjectTextTree TextTree = new GameObjectTextTree();

        public void Draw()
        {
            if (this.isShowing)
            {
                CacheManager.Draw(this.BackgroundTexture, this.BackgroundDisplayPosition, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.2f);
                this.RichText.Draw(0.1999f);
            }
        }

        public void DrawButton(float depth)
        {
            if (this.isButtonShowing && (this.ButtonDisplayTexture != null))
            {
                CacheManager.Draw(this.ButtonDisplayTexture, this.ButtonDisplayPosition, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, depth);
            }
        }

        internal void Initialize()
        {
            
        }

        private void screen_OnButtonMouseLeftDown(Point position)
        {
            if (StaticMethods.PointInRectangle(position, this.ButtonDisplayPosition))
            {
                this.SetDisplayOffset(ShowPosition.Center);
            }
        }

        private void screen_OnButtonMouseMove(Point position, bool leftDown)
        {
            if (StaticMethods.PointInRectangle(position, this.ButtonDisplayPosition))
            {
                this.ButtonDisplayTexture = this.ButtonSelectedTexture;
            }
            else
            {
                this.ButtonDisplayTexture = this.ButtonTexture;
            }
        }

        private void screen_OnMouseLeftDown(Point position)
        {
            if (this.RichText.CurrentPageIndex == (this.RichText.PageCount - 1))
            {
                this.IsShowing = false;
            }
            else
            {
                this.RichText.NextPage();
            }
        }

        private void screen_OnMouseMove(Point position, bool leftDown)
        {
        }

        private void screen_OnMouseRightUp(Point position)
        {
            this.IsShowing = false;
        }

        internal void SetDisplayOffset(ShowPosition showPosition)
        {
            Rectangle rectDes = new Rectangle(0, 0, Session.MainGame.mainGameScreen.viewportSize.X, Session.MainGame.mainGameScreen.viewportSize.Y);
            Rectangle rect = new Rectangle(0, 0, this.BackgroundSize.X, this.BackgroundSize.Y);
            switch (showPosition)
            {
                case ShowPosition.Center:
                    rect = StaticMethods.GetCenterRectangle(rectDes, rect);
                    break;

                case ShowPosition.Top:
                    rect = StaticMethods.GetTopRectangle(rectDes, rect);
                    break;

                case ShowPosition.Left:
                    rect = StaticMethods.GetLeftRectangle(rectDes, rect);
                    break;

                case ShowPosition.Right:
                    rect = StaticMethods.GetRightRectangle(rectDes, rect);
                    break;

                case ShowPosition.Bottom:
                    rect = StaticMethods.GetBottomRectangle(rectDes, rect);
                    break;

                case ShowPosition.TopLeft:
                    rect = StaticMethods.GetTopLeftRectangle(rectDes, rect);
                    break;

                case ShowPosition.TopRight:
                    rect = StaticMethods.GetTopRightRectangle(rectDes, rect);
                    break;

                case ShowPosition.BottomLeft:
                    rect = StaticMethods.GetBottomLeftRectangle(rectDes, rect);
                    break;

                case ShowPosition.BottomRight:
                    rect = StaticMethods.GetBottomRightRectangle(rectDes, rect);
                    break;
            }
            this.DisplayOffset = new Point(rect.X, rect.Y);
            this.RichText.DisplayOffset = this.DisplayOffset;
        }

        public void Update()
        {
        }

        private Rectangle BackgroundDisplayPosition
        {
            get
            {
                return new Rectangle(this.DisplayOffset.X, this.DisplayOffset.Y, this.BackgroundSize.X, this.BackgroundSize.Y);
            }
        }

        internal Rectangle ButtonDisplayPosition
        {
            get
            {
                return new Rectangle(this.ButtonDisplayOffset.X, this.ButtonDisplayOffset.Y, this.ButtonSize.X, this.ButtonSize.Y);
            }
        }

        public bool IsButtonShowing
        {
            get
            {
                return this.isButtonShowing;
            }
            set
            {
                this.isButtonShowing = value;
                if (value)
                {
                    Session.MainGame.mainGameScreen.OnMouseLeftDown += new Screen.MouseLeftDown(this.screen_OnButtonMouseLeftDown);
                    Session.MainGame.mainGameScreen.OnMouseMove += new Screen.MouseMove(this.screen_OnButtonMouseMove);
                }
                else
                {
                    Session.MainGame.mainGameScreen.OnMouseMove -= new Screen.MouseMove(this.screen_OnButtonMouseMove);
                    Session.MainGame.mainGameScreen.OnMouseLeftDown -= new Screen.MouseLeftDown(this.screen_OnButtonMouseLeftDown);
                }
            }
        }

        public bool IsShowing
        {
            get
            {
                return this.isShowing;
            }
            set
            {
                this.isShowing = value;
                if (value)
                {
                    Session.MainGame.mainGameScreen.PushUndoneWork(new UndoneWorkItem(UndoneWorkKind.Dialog, UndoneWorkSubKind.None));
                    Session.MainGame.mainGameScreen.OnMouseMove += new Screen.MouseMove(this.screen_OnMouseMove);
                    Session.MainGame.mainGameScreen.OnMouseLeftDown += new Screen.MouseLeftDown(this.screen_OnMouseLeftDown);
                    Session.MainGame.mainGameScreen.OnMouseRightUp += new Screen.MouseRightUp(this.screen_OnMouseRightUp);
                    this.RichText.SetGameObjectTextBranch(null, this.CurrentBranch);
                }
                else
                {
                    if (Session.MainGame.mainGameScreen.PopUndoneWork().Kind != UndoneWorkKind.Dialog)
                    {
                        throw new Exception("The UndoneWork is not a Help Dialog.");
                    }
                    Session.MainGame.mainGameScreen.OnMouseMove -= new Screen.MouseMove(this.screen_OnMouseMove);
                    Session.MainGame.mainGameScreen.OnMouseLeftDown -= new Screen.MouseLeftDown(this.screen_OnMouseLeftDown);
                    Session.MainGame.mainGameScreen.OnMouseRightUp -= new Screen.MouseRightUp(this.screen_OnMouseRightUp);
                }
            }
        }
    }
}

