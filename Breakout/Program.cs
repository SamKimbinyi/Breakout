using System;
using System.Drawing;
using SdlDotNet.Graphics;
using SdlDotNet.Core;
using SdlDotNet.Input;
using System.Diagnostics;

namespace Breakout
{

    public static class Program
    {

          const int FRAME_WIDTH = 640; 
        const int FRAME_HEIGHT = 480;
        const int COLOUR_DEPTH = 32;
        const bool FRAME_RESIZABLE = false;
        const bool FRAME_FULLSCREEN = false;
        const bool USE_OPENGL = false;
        const bool USE_HARDWARE = true;

        public static bool gameStart = false;

        public static int BatPos =  FRAME_WIDTH/2;


        public static int[] BallPos = new int[2] { FRAME_WIDTH / 2, FRAME_HEIGHT / 2 };
        public static int[] ballVelocity = new int [2] {1,1};

        // STATE
        // Keep the state of the elements of the game here (variables hold state).
        // ...

        // This procedure is called (invoked) for each window refresh (FPS - about 40 times a second)
        public static void onTick(object sender, TickEventArgs args)
        {

            // STEP
            // Update the automagic elements and enforce the rules of the game here.
            // ...
            update();
            // DRAW
            // Draw the new view of the game based on the state of the elements here.
            // ...

            draw();
            // drawSprite(Sprite.ice,600,40);

            // ANIMATE 
            // Step the animation frames ready for the next tick
            // ...

            // REFRESH
            // Tranfer the new view to the screen for the user to see.
            video.Update();

        }




        public static void update()
        {

            #region UpdateBat

            BatPos = Mouse.MousePosition.X;
            #endregion

            #region UpdateBall
            if (!gameStart)
            {

                BallPos[0] = BatPos + 50;
                BallPos[1] = FRAME_HEIGHT - 52;

            }

            BallPos[0] += ballVelocity[0];
            BallPos[1] -= ballVelocity[1];

            #endregion


            #region BatCollisionDetection

            //X

            if (BallPos[0] >= FRAME_WIDTH - 10 || BallPos[0] <= 0)
            {

                ballVelocity[0] *= -1;
            }

            //Y

            if (BallPos[1] >= FRAME_HEIGHT - 10 || BallPos[1] <= 0)
            {

                ballVelocity[1] *= -1;
            }



            if (BallPos[0] >= BatPos && BallPos[0] <= BatPos + 120 && BallPos[1] <= FRAME_HEIGHT - 30 && BallPos[1] >= FRAME_HEIGHT - 30 + 32) {

                ballVelocity[0] *= -1;
                ballVelocity[1] *= -1;
            }
           
            #endregion

            Debug.WriteLine("Ball X: {0} Ball Y:{1}  \t Bat X:{2}",BallPos[0].ToString(),BallPos[1].ToString(),BatPos.ToString());
        }

        public static void draw()
        {
            drawBackground();

            #region DrawBat

            drawSprite(Sprite.bat_medium, BatPos, FRAME_HEIGHT - 30);


            #endregion


            #region DrawBall

            


            drawSprite(Sprite.ball_small, BallPos[0], BallPos[1]);

            #endregion

            drawSprite(Sprite.fire, 600, 6);


        }




        // this procedure is called when the mouse is moved
       public static void onMouseMove(object sender, SdlDotNet.Input.MouseMotionEventArgs args)
        {
            
        }

        // this procedure is called when a mouse button is pressed or released
       public static void onMouseButton(object sender, SdlDotNet.Input.MouseButtonEventArgs args)
        {
            if (!gameStart && args.ButtonPressed) {
                gameStart = true;
                
            
            } 

        }

        // this procedure is called when a key is pressed or released
       public static void onKeyboard(object sender, SdlDotNet.Input.KeyboardEventArgs args)
        {
            // ...
        }

        // --------------------------
        // ----- GAME Utilities -----  You can use these, don't change them.
        // --------------------------

        // draw the background image onto the frame
        public static void drawBackground()
        {
            video.Blit(imgBackground);
        }

        // draw the sprite image onto the frame
        // Sprite sprite - which sprite to draw
        // int x, int y - the co-ordinates of where to draw the sprite on the frame.
        public static void drawSprite(Sprite sprite, int x, int y)
        {
            video.Blit(imgSpriteSheet, new Point(x, y), sprite_sheet_cut[(int)sprite]);
        }

        // ============================================
        // ============ Here Be Dragons ===============
        // ============================================
        // == Don't invoke or modify the code below! == 
        // ============================================

        // ------ Break-Out Graphics Engine - -------

        // -- APPLICATION ENTRY POINT --

        public static void Main()
        {

            // Create display surface.
            video = Video.SetVideoMode(FRAME_WIDTH, FRAME_HEIGHT, COLOUR_DEPTH, FRAME_RESIZABLE, USE_OPENGL, FRAME_FULLSCREEN, USE_HARDWARE);
            Video.WindowCaption = "Breakout";
            Video.WindowIcon(new Icon(@"breakout.ico"));

            // invoke application initialisation subroutine
            setup();

            // register event handler subroutines
            Events.Tick += new EventHandler<TickEventArgs>(onTick);
            Events.Quit += new EventHandler<QuitEventArgs>(onQuit);
            Events.KeyboardDown += new EventHandler<SdlDotNet.Input.KeyboardEventArgs>(onKeyboard);
            Events.KeyboardUp += new EventHandler<SdlDotNet.Input.KeyboardEventArgs>(onKeyboard);
            Events.MouseButtonDown += new EventHandler<SdlDotNet.Input.MouseButtonEventArgs>(onMouseButton);
            Events.MouseButtonUp += new EventHandler<SdlDotNet.Input.MouseButtonEventArgs>(onMouseButton);
            Events.MouseMotion += new EventHandler<SdlDotNet.Input.MouseMotionEventArgs>(onMouseMove);

            Mouse.ShowCursor = false;

            // while not quit do process events
            Events.TargetFps = 60;
            Events.Run();
        }

        // This procedure is called after the video has been initialised but before any events have been processed.
        public static void setup()
        {

            // Load Art

            imgBackground = new Surface(@"breakout_bg.png").Convert(video, true, false);

            imgSpriteSheet = new Surface(@"breakout_sprites.png");

            
            // Specify where each sprite is in the sprite-sheet

            // Generate sprite_sheet_cut Rectangles for the first 40 small (40x40) sprites
            {
                int s = (int)Sprite.red;  // sprite_sheet_cut index
                int x = 0;
                int y = 0;
                for (int i = 0; i < 4; ++i)
                {
                    for (int j = 0; j < 10; ++j)
                    {
                        sprite_sheet_cut[s] = new Rectangle(x, y, 40, 40);
                        s = s + 1;
                        x = x + 40;
                    }
                    x = 0;
                    y = y + 40;
                }
            }

            // Generate sprite_sheet_cut Rectangles for the 16 coins (24x24) sprites
            {
                int s = (int)Sprite.coin_00; // sprite_sheet_cut index
                int x = 0;
                int y = 376;
                for (int i = 0; i < 16; ++i)
                {
                    sprite_sheet_cut[s] = new Rectangle(x, y, 24, 24);
                    s = s + 1;
                    x = x + 24;
                }
            }

            // Odd Shape Sprites
            sprite_sheet_cut[(int)Sprite.bat_small] = new Rectangle(0, 200, 104, 32);
            sprite_sheet_cut[(int)Sprite.bat_medium] = new Rectangle(0, 240, 120, 32);
            sprite_sheet_cut[(int)Sprite.bat_large] = new Rectangle(0, 280, 136, 32);

            sprite_sheet_cut[(int)Sprite.ball_small] = new Rectangle(160, 200, 16, 16);
            sprite_sheet_cut[(int)Sprite.ball_small] = new Rectangle(160, 240, 24, 24);

            sprite_sheet_cut[(int)Sprite.cross] = new Rectangle(200, 160, 80, 80);
            sprite_sheet_cut[(int)Sprite.flare] = new Rectangle(320, 160, 80, 80); ;
            sprite_sheet_cut[(int)Sprite.swirl] = new Rectangle(200, 280, 80, 80); ;
            sprite_sheet_cut[(int)Sprite.spiral] = new Rectangle(320, 280, 80, 80); ;
        }

        // This procedure is called when the event loop receives an exit event (window close button is pressed)
       public static void onQuit(object sender, QuitEventArgs args)
        {
            Events.QuitApplication();
        }

        // -- DATA --

      

      public  static Surface video;  // the window on the display

        public static Surface imgBackground;
        public static Surface imgSpriteSheet;

        // Sprite Identifiers

        public enum Sprite
        {
            red, purple, yellow,
            blue1, blue2, green1, green2,
            star,
            rubble, stone, bricks,
            left_rubble, right_rubble, left_stone, right_stone,
            iron_mask, jade_mask, opal_mask, ruby_mask,
            brick_rubble, brick_stone, tile_stone,
            jade, ruby, opal,
            ice, fire,
            jar,
            smooth_rock, dented_rock, smashed_rock,
            black_galaxy, blue_galaxy,
            blue_ring,
            orb_fire, orb_water,
            totem, totem_wink,
            left, right, up, down,
            bat_small, bat_medium, bat_large,
            ball_small, ball_large,
            cross, flare, swirl, spiral,
            coin_00, coin_01, coin_02, coin_03,
            coin_04, coin_05, coin_06, coin_07,
            coin_08, coin_09, coin_10, coin_11,
            coin_12, coin_13, coin_14, coin_15
        };

        // All the sprites come from one large image, this is called a 
        // sprite-sheet. For each sprite, we need to know which part (Rectangle)
        // of the larger sheet to use.  We store these rectangles in a variable
        // called sprite_sheet_cut for later use.
        // It is easier and more efficient to store one big image rather than lots
        // of little ones, especially if they are stored in the graphics memory.
        // (You can find the sprite-sheet for many games online - be careful of copyright!)
        // *The Rectangles are stored in the same sequence as the Sprite enum.
        public static Rectangle[] sprite_sheet_cut = new Rectangle[40 + 16 + 11];

    }
}

