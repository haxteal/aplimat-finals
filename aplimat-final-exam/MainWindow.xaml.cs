using aplimat_core.utilities;
using aplimat_final_exam.Models;
using aplimat_final_exam.Utilities;
using SharpGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace aplimat_final_exam
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Vector3 mousePos = new Vector3();

        #region Initialization
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenGLControl_OpenGLInitialized(object sender, SharpGL.SceneGraph.OpenGLEventArgs args)
        {
            OpenGL gl = args.OpenGL;

            gl.Enable(OpenGL.GL_DEPTH_TEST);

            float[] global_ambient = new float[] { 0.5f, 0.5f, 0.5f, 1.0f };
            float[] light0pos = new float[] { 0.0f, 5.0f, 10.0f, 1.0f };
            float[] light0ambient = new float[] { 0.2f, 0.2f, 0.2f, 1.0f };
            float[] light0diffuse = new float[] { 0.3f, 0.3f, 0.3f, 1.0f };
            float[] light0specular = new float[] { 0.8f, 0.8f, 0.8f, 1.0f };

            float[] lmodel_ambient = new float[] { 0.2f, 0.2f, 0.2f, 1.0f };
            gl.LightModel(OpenGL.GL_LIGHT_MODEL_AMBIENT, lmodel_ambient);

            gl.LightModel(OpenGL.GL_LIGHT_MODEL_AMBIENT, global_ambient);
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_POSITION, light0pos);
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_AMBIENT, light0ambient);
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_DIFFUSE, light0diffuse);
            gl.Light(OpenGL.GL_LIGHT0, OpenGL.GL_SPECULAR, light0specular);
            gl.Disable(OpenGL.GL_LIGHTING);
            gl.Disable(OpenGL.GL_LIGHT0);

            gl.ShadeModel(OpenGL.GL_SMOOTH);
        }

        #endregion

        #region Mouse Func
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            var position = e.GetPosition(this);
            mousePos.x = (float)position.X - (float)Width / 2.0f;
            mousePos.y = -((float)position.Y - (float)Height / 2.0f);
        }
        #endregion
        
        private void ManageKeyPress()
        {

        }

        private float speed = 0.1f;
        private CubeMesh myCube = new CubeMesh();

        int cnt = 0;
        private Randomizer fireSpeed = new Randomizer(0, 5);
        private Randomizer colorOn = new Randomizer(0, 1);
        private Randomizer massRand = new Randomizer(2, 5);
        private Randomizer xPos = new Randomizer(0, 50);
        private List<Attractor> rocks = new List<Attractor>();
        private List<Attractor> rocks1 = new List<Attractor>();
        

        #region initialize land/water
        private Liquid lake = new Liquid(0, 0, 80, 50, 0.8f );

        private CubeMesh land1 = new CubeMesh()
        {
            Position = new Vector3(82, -22, 0),
            Mass = 50
        };

        private CubeMesh land2 = new CubeMesh()
        {
            Position = new Vector3(-82, -22, 0),
            Mass = 50
        };

        private CubeMesh land3 = new CubeMesh()
        {
            Position = new Vector3(0, -53, 0),
            Mass = 100
        };
        #endregion
        float lakeBoxB = -40f;
        float lakeBoxL = -69f;
        float lakeBoxR = 69f;
        float landBoxT = 3.5f;


        #region initialize sample cubes
        private CubeMesh sample = new CubeMesh()
        {
            Position = new Vector3(-75,15,0),
            Mass = 2f
        };

        private CubeMesh sample2 = new CubeMesh()
        {
            Position = new Vector3(75, 15, 0),
            Mass = 2f
        };
        #endregion
        

        private void OpenGLControl_OpenGLDraw(object sender, SharpGL.SceneGraph.OpenGLEventArgs args)
        {
            this.Title = "APLIMAT Final Exam";
            OpenGL gl = args.OpenGL;

            // Clear The Screen And The Depth Buffer
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

            // Move Left And Into The Screen
            gl.LoadIdentity();
            gl.Translate(0.0f, 0.0f, -100.0f);

            //land and water
            #region land and water
            lake.Draw(gl);

            gl.Color(139.0, 69.0, 19.0);
            land1.Draw(gl);
            land1.Scale = new Vector3(1 * land1.Mass / 4, 1 * land1.Mass / 2, 0 );

            land2.Draw(gl);
            land2.Scale = new Vector3(1 * land2.Mass / 4, 1 * land2.Mass / 2, 0);

            land3.Draw(gl);
            land3.Scale = new Vector3(1 * land3.Mass / 1, 1 * land3.Mass / 8, 0);
            #endregion

            #region sample for testing
            //gl.Color(1.0, 0, 0);
            //sample.Draw(gl);
            //sample2.Draw(gl);
            //physicsSimulation(sample);
            //physicsSimulation(sample2);
            #endregion

            //makes rocks list
            //positioned at the left side
            cnt++;
            if (cnt >= 5)
            {
                rocks1.Add(new Attractor()
                {
                    Position = new Vector3(-75, 15, 0),
                    Mass = (float)massRand.GenerateDouble(),
                    colorR = (float)colorOn.GenerateDouble(),
                    colorG = (float)colorOn.GenerateDouble(),
                    colorB = (float)colorOn.GenerateDouble()
                });
                
                cnt = 0;
            }
            
            foreach (var cube in rocks1)
            {
                gl.Color(cube.colorR, cube.colorG, cube.colorB);
                cube.Draw(gl);
                cube.Scale = new Vector3(1 * cube.Mass / 2, 1 * cube.Mass / 2, 1 * cube.Mass / 2);

                physicsSimulation(cube);

                //foreach (var rock2 in rocks1)
                //{
                //    if (lake.Contains(rock2))
                //        cube.ApplyForce(rock2.CalculateAttraction(cube));
                //}
            }
            
            //if rocks >= 100, removes first in rocks
            if (rocks1.Count >= 100)
            {
                rocks1.RemoveAt(0);
            }

            #region idea2
            //cnt++;
            //if (cnt >= 1)
            //{
            //    rocks.Add(new Attractor()
            //    {
            //        Position = new Vector3((float)Gaussian.Generate(0, 50), 30, 0),
            //        Mass = (float)massRand.GenerateDouble(),
            //        colorR = (float)colorOn.GenerateDouble(),
            //        colorG = (float)colorOn.GenerateDouble(),
            //        colorB = (float)colorOn.GenerateDouble()
            //    });
            //    cnt = 0;
            //}

            //foreach (var cube in rocks)
            //{
            //    gl.Color(cube.colorR, cube.colorG, cube.colorB);
            //    cube.Draw(gl);
            //    cube.Scale = new Vector3(1 * cube.Mass / 2, 1 * cube.Mass / 2, 1 * cube.Mass / 2);

            //    physicsSimulation(cube);

            //    foreach (var rock2 in rocks)
            //    {
            //        if (lake.Contains(rock2))
            //            cube.ApplyForce(rock2.CalculateAttraction(cube));
            //    }
            //}

            //if (rocks.Count >= 200)
            //{
            //    rocks.RemoveAt(0);
            //}

            #endregion

            #region keypress

            if (Keyboard.IsKeyDown(Key.W))
            { 
                myCube.ApplyForce(Vector3.Up * speed);
            }

            if (Keyboard.IsKeyDown(Key.D))
            {
                myCube.ApplyForce(Vector3.Right * speed);
            }

            if (Keyboard.IsKeyDown(Key.A))
            {
                myCube.ApplyForce(Vector3.Left * speed);
            }
            if (Keyboard.IsKeyDown(Key.S))
            {
                myCube.ApplyForce(Vector3.Down * speed);
            }

            if (Mouse.LeftButton == MouseButtonState.Pressed)
            {

            }

            #endregion
        }
        


        private void physicsSimulation(CubeMesh cube)
        {
            cube.ApplyGravity();
            //(float)Gaussian.Generate(0, 50)
            Vector3 fire = new Vector3((float)fireSpeed.GenerateDouble(), (float)fireSpeed.GenerateDouble(), 0);
            Vector3 fire2 = new Vector3((float)fireSpeed.GenerateDouble() * -1 , (float)fireSpeed.GenerateDouble() * -1, 0);

            if (lake.Contains(cube))
            {
                var dragForce = lake.CalculateDragForce(cube);
                cube.ApplyForce(dragForce);

                if (cube.Position.y <= lakeBoxB)
                {
                    cube.Position.y = lakeBoxB;
                    cube.Velocity.y *= -1;
                }

                if (cube.Position.x >= lakeBoxR)
                {
                    cube.Position.x = lakeBoxR;
                    cube.Velocity.x *= -1;
                }

                if (cube.Position.x <= lakeBoxL)
                {
                    cube.Position.x = lakeBoxL;
                    cube.Velocity.x *= -1;
                }
            }

            if (!lake.Contains(cube))
            {
                //cube.ApplyForce(fire);
                //cube.ApplyGravity();
                if (cube.Position.y <= landBoxT)
                {
                    if (cube.Position.x <= lakeBoxL)
                    {
                        cube.ApplyForce(fire);
                        cube.Position.y = landBoxT;
                        cube.Velocity.y *= -1;
                        cube.Velocity.x *= -1;
                    }
                    if (cube.Position.x >= lakeBoxR)
                    {
                        cube.ApplyForce(fire2);
                        cube.Position.y = landBoxT;
                        cube.Velocity.y *= -1;
                        cube.Velocity.x *= -1;
                    }
                }


                if (cube.Position.x >= lakeBoxL && cube.Position.x <= lakeBoxR)
                {
                    if (cube.Position.y <= landBoxT)
                    {
                        if (cube.Position.x <= lakeBoxL)
                        {
                            cube.Position.x = lakeBoxL;
                            cube.Velocity.x *= -1;
                        }
                        if (cube.Position.x >= lakeBoxR)
                        {
                            cube.Position.x = lakeBoxR;
                            cube.Velocity.x *= -1;
                        }
                    }
                }
            }
        }
    }
}
