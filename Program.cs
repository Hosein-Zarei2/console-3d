using System;
using System.Collections.Generic; 

public class Program
{

    public static void Main(String[] args)
    {

        // (window & world)***
        Window win = new Window(35, 36);
        World3D world = new World3D(2/*total objects*/, new Vector3(35, 36, 36)/*world size*/);
        

        /*backgRound & color*/
        E.empty = " ";
        win.COLORS["#"] = ConsoleColor.Red;
        win.COLORS["o"] = ConsoleColor.Blue;


        //(objects)***
	Object ox;

        // "cube", "line", "sphere", "custom"
        Object o1 = new Object("cube");
        o1.rotationPoint = new Vector3(10,10,10);
        o1.fill = "#";
        //(sortRotate) 
        o1.sortRotate[0] = "Y";//first 
        o1.sortRotate[1] = "X";//second
        o1.sortRotate[2] = "Z";//third
        o1.position = new Vector3(5, 5, 8);
        o1.rotation = new Vector3(0, 0, 0);
        o1.scale.x = 21;
        o1.scale.y = 21;
        o1.scale.z = 21;
	  
        Object o2 = new Object("line");
        o2.rotationPoint = new Vector3(0,0,0);
        o2.fill = "o";
        //(sortRotate) 
        o2.sortRotate[0] = "Y";//first 
        o2.sortRotate[1] = "X";//second
        o2.sortRotate[2] = "Z";//third
        o2.position = new Vector3(15, 15, 2);
        o2.rotation = new Vector3(0, 0, 90);
        o2.scale.x = 10;

	ox = o1;
        

        /*o1.customPoints = new Vector3[1];
        o1.customPoints[0] = new Vector3(0, 0, 0);*/



        //(add objects to world)***

        world.add(o1, 0);
        world.add(o2, 1);


        //(Orthographic Camera)***

        OrthographicCamera cam = new OrthographicCamera(world, win);
        cam.position = new Vector3(0, 0, 0);

		

        //(show in window)***
        

        while (true)
        {
            System.Console.Clear();
            world.update();
            cam.update();
            win.show(cam);
            

            switch ( Console.ReadKey().Key )
            {
                case ConsoleKey.LeftArrow:
                    ox.rotation.y += 5;
                break;
                    
                case ConsoleKey.RightArrow:
                    ox.rotation.y -= 5;
                break;

                case ConsoleKey.UpArrow:
                    ox.rotation.x -= 5;
                break;
                    
                case ConsoleKey.DownArrow:
                    ox.rotation.x += 5;
                break;

                case ConsoleKey.D:
                    ox.position.x += 1;
                break;
		    
                case ConsoleKey.A:
                    ox.position.x -= 1;
                break;
		    
                case ConsoleKey.W:
                    ox.position.y -= 1;
                break;
		    
                case ConsoleKey.S:
                    ox.position.y += 1;
                break;
		    
                case ConsoleKey.Q:
                    return;
                break;

		case ConsoleKey.E:
                    if(ox == o2){ox = o1;}else{ox = o2;}
                break;
            }

        }

    }

    
}



// version 1.0



class Window
{
    public String[,] window;
    char[] CHARS_TO_CUT = {' ','\n'};
    public Dictionary<string, ConsoleColor> COLORS = new Dictionary<string, ConsoleColor>();
    public int x;
    public int y;
    public Window(int x, int y)
    {
        this.x = x;
        this.y = y;
        window = new String[y,x];

    }



    public void show(OrthographicCamera cam)
    {
        int lx = window.GetLength(1);
        int ly = window.GetLength(0);

        int ix = 0;
        int iy = 0;
        string[] wa = new string[lx*ly];
        int i = 0;

        while (iy < ly)
        {
        
            while (ix < lx)
            {
                try
                {
                    if(cam.window[iy,ix] == null)
                    {
                        if(ix+1 == lx)
                        {
                            wa[i] = E.empty + " \n";
                        }else
                        {
                            wa[i] = E.empty + " ";
                        }
                    }else
                    {
                        if (ix + 1 == lx)
                        {
                            wa[i] = cam.window[iy, ix] + " \n";
                        }
                        else
                        {
                            wa[i] = cam.window[iy, ix] + " ";
                        }
                    }
                    
                }
                catch (Exception e) {}
                ix++;
                i++;
            }
            ix = 0;
            iy++;
        }
	  
	  foreach(string wai in wa)
	  {
		if(wai != " " && wai != "  " && wai != "  \n")
		{
		  Console.ForegroundColor = COLORS[(wai.TrimEnd(CHARS_TO_CUT))];
		}
		Console.Write(wai);
	  }

    }





}



class OrthographicCamera
{
    public Vector3 position = new Vector3();
    public Vector3 rotation = new Vector3();
    public Object[] objects;
    public String[,] window;
    public World3D world;
    Window win;


    public OrthographicCamera(World3D world, Window win)
    {
        this.win = win;
        this.world = world;
        objects = world.objs;
        this.window = new String[win.y,win.x];
    }

    public void update()
    {
        foreach (Object obj in objects)
        {
            window = new Converter(obj, world, win, position, rotation).To2D();
            // convert 3D arr to 2D arr
        }
    }


}



class World3D
{
    public Object[] objs;
    public String[,,] positions;
    public Vector3 size;

    public World3D(int count_objects, Vector3 size)
    {
        objs = new Object[count_objects];
        positions = new String[size.y,size.x,size.z];
        this.size = size;
    }

    public Object[] objects()
    {
        return objs;
    }

    public void add(Object obj, int id)
    {
        objs[id] = obj;
        switch (obj.type)
        {
            case "cube":
                obj.cube(this);
                break;
            case "line":
                obj.line_in3D(this);
                break;
            case "sphere":
                obj.sphere(this);
                break;
            case "custom":
                obj.custom_obj(this);
                break;
        }
    }
    public void update()
    {
        
        this.positions = new string[this.size.y, this.size.x, this.size.z];

        foreach (Object obj in this.objs)
        {
            switch (obj.type)
            {
                case "cube":
                    obj.cube(this);
                    break;
                case "line":
                    obj.line_in3D(this);
                    break;
                case "sphere":
                obj.sphere(this);
                break;
                case "custom":
                    obj.custom_obj(this);
                    break;
            }
        }
    }

}



class Vector3
{
    public int x;
    public int y;
    public int z;
    public Vector3()
    {
        this.x = 0;
        this.y = 0;
        this.z = 0;
    }

    public Vector3(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

}



class Object
{
    public Vector3 position = new Vector3();
    public Vector3 rotation = new Vector3();
    public Vector3 rotationPoint = new Vector3();
    public String[] sortRotate = new String[3];
    public Vector3[] customPoints;
    public String fill = "o";

    public Vector3 scale = new Vector3();
    public String type;

    public Object(String type)
    {
        this.type = type;
    }



    public void cube(World3D world)
    {

        int sx = scale.x;
        int sy = scale.y;
        int sz = scale.z;

        Vector3[] cube_points = new Vector3[sx * sy * sz];


        int n = 0;
        for (int isx = 0; isx < sx; isx++)
        {
            for (int isy = 0; isy < sy; isy++)
            {
                for (int isz = 0; isz < sz; isz++)
                {


                    if ((isx > 0 && isx < sx - 1) && (isy > 0 && isy < sy - 1))
                    {
                        cube_points[n] = new Vector3(0, 0, 0);
                        n++;
                    }
                    else
                    {
                        if ((isz > 0 && isz < sz - 1) && (isx > 0 && isx < sx - 1))
                        {

                            cube_points[n] = new Vector3(0, 0, 0);
                            n++;

                        }
                        else
                        {
                            if ((isy > 0 && isy < sy - 1) && (isz > 0 && isz < sz - 1))
                            {
                                cube_points[n] = new Vector3(0, 0, 0);
                                n++;
                            }
                            else
                            {
                                cube_points[n] = new Vector3(isx, isy, isz);
                                n++;
                            }
                        }
                    }


                }
            }
        }






        Vector3[] new_point = Math2.rotatePoint(cube_points, this,this.rotationPoint);

        int c = 0;
        while (c < cube_points.Length)
        {


            int x = new_point[c].x;
            int y = new_point[c].y;
            int z = new_point[c].z;



            try
            {
                world.positions[y + position.y,x + position.x,z + position.z] = fill;
            }
            catch (Exception e) {}

            c++;
        }



    }



    public void line_in3D(World3D world)
    {


        int sx = scale.x;

        Vector3[] line_points = new Vector3[sx];


        //x
        int i = 0;
        while (i < sx)
        {
            line_points[i] = new Vector3(i, 0, 0);
            i++;
        }



        Vector3[] new_point = Math2.rotatePoint(line_points, this,this.rotationPoint);
        int c = 0;
        while (c < line_points.Length)
        {


            int x = new_point[c].x;
            int y = new_point[c].y;
            int z = new_point[c].z;

            try
            {
                world.positions[y + position.y,x + position.x,z + position.z] = fill;
            }
            catch (Exception e) {}

            c++;

        }



    }


    public void sphere(World3D world)
    {


        int sx = scale.x;
        int sy = scale.y;
        int sz = scale.z;
        double posx,posy,posz;

        Vector3[] sphere_points = new Vector3[sx*sy];
        int i_f = 0;
        while(i_f<sphere_points.Length)
        {
            sphere_points[i_f] = new Vector3(0,0,0);
            i_f++;
        }

        //x and y
        int i = 0;
        while (i < 10)
        {
            posx = Math.Cos(Math2.ToRadian(i*10))*sx;
            posy = Math.Sin(Math2.ToRadian(i*10))*sx;
            sphere_points[i] = new Vector3((int)posx, -(int)posy, 0);
            sphere_points[i+10] = new Vector3(-(int)posx, -(int)posy, 0);
            sphere_points[i+20] = new Vector3(-(int)posx, (int)posy, 0);
            sphere_points[i+30] = new Vector3((int)posx, (int)posy, 0);
            i += 1;
        }

        //z and y
        i = 0;
        while (i < 10)
        {
            posy = Math.Cos(Math2.ToRadian(i*10))*sx;
            posz = Math.Sin(Math2.ToRadian(i*10))*sx;
            sphere_points[i+40] = new Vector3(0, (int)posy, -(int)posz);
            sphere_points[i+50] = new Vector3(0, -(int)posy, -(int)posz);
            sphere_points[i+60] = new Vector3(0, -(int)posy, (int)posz);
            sphere_points[i+70] = new Vector3(0, (int)posy, (int)posz);
            i += 1;
        }



        Vector3[] new_point = Math2.rotatePoint(sphere_points, this,this.rotationPoint);
        int c = 0;
        while (c < sphere_points.Length)
        {


            int x = new_point[c].x;
            int y = new_point[c].y;
            int z = new_point[c].z;

            try
            {
                world.positions[y + position.y,x + position.x,z + position.z] = fill;
            }
            catch (Exception e) {}

            c++;

        }



    }



    public void custom_obj(World3D world)
    {

        
        Vector3[] new_point = Math2.rotatePoint(this.customPoints, this,this.rotationPoint);
        


        int i = 0;
        while (i < customPoints.Length)
        {


            int x = new_point[i].x;
            int y = new_point[i].y;
            int z = new_point[i].z;

            try
            {
                world.positions[y + position.y,x + position.x,z + position.z] = fill;
            }
            catch (Exception e) {}

            i++;

        }

    }



}



class Converter
{
    public String[,] window;
    public Vector3 camPos;
    public Vector3 camRot;
    public World3D world;

    public Converter(Object obj, World3D world, Window win, Vector3 camPos, Vector3 camRot)
    {
        this.camPos = camPos;
        this.camRot = camRot;
        this.world = world;
        window = new String[win.y,win.x];
    }

    public String[,] To2D()
    {



        int ix = 0;
        int iy = 0;
        int iz = 0;
        int ixc = window.GetLength(1);
        int iyc = window.GetLength(0);
        int izc = world.positions.GetLength(2);

        while (iz < izc)
        {
            while (iy < iyc)
            {
                while (ix < ixc)
                {
                    try
                    {
                        if (window[iy,ix] == E.empty || window[iy,ix] == null)
                        {
                            window[iy,ix] = world.positions[iy + camPos.y,ix + camPos.x,iz + camPos.z];
                        }
                    }
                    catch (Exception e){}
                    ix++;
                }
                ix = 0;
                iy++;

            }
            iy = 0;
            iz++;
        }




        return window;
    }







}



class Math2
{

    public static int divide(int a, int b)
    {

        if (b == 0)
        {
            return 0;
        }
        else
        {
            return a / b;
        }

    }
    public static double divide(double a, double b)
    {

        if (b == 0)
        {
            return 0;
        }
        else
        {
            return a / b;
        }

    }


    public static double ToRadian(double deg)
    {
        return Math.PI / 180 * deg;
    }






    public static Vector3[] rotatePoint(Vector3[] points, Object obj,Vector3 rp)
    {


        double ry = Math2.ToRadian(obj.rotation.y);


        double rx = Math2.ToRadian(obj.rotation.x);


        double rz = Math2.ToRadian(obj.rotation.z);




        int i1 = 0;
        int i1c = points.Length;

        while (i1 < i1c)
        {

            int sri = 0;
            while (sri < 3)
            {
                switch (obj.sortRotate[sri])
                {
                    case "Y":
                        // rotate Y
                        double dxz = Math.Sqrt(Math.Pow(points[i1].z-rp.z, 2) + Math.Pow(points[i1].x-rp.x, 2));
                        
                        if (points[i1].x-rp.x < 0)
                        {
                            double degxz = Math.PI - Math.Asin( Math2.divide(points[i1].z-rp.z , dxz) );

                            int xr = (int)Math.Round((Math.Cos(degxz + ry) * dxz));
                            int zr = (int)Math.Round((Math.Sin(degxz + ry) * dxz));

                            points[i1].x = xr + rp.x;
                            points[i1].z = zr + rp.z;
                        }
                        else
                        {
                            double degxz = Math.Asin( Math2.divide(points[i1].z-rp.z , dxz) );

                            int xr = (int)Math.Round((Math.Cos(degxz + ry) * dxz));
                            int zr = (int)Math.Round((Math.Sin(degxz + ry) * dxz));

                            points[i1].x = xr + rp.x;
                            points[i1].z = zr + rp.z;

                        }
                        break;
                    case "X":

                        // rotate X
                        double dzy = Math.Sqrt(Math.Pow(points[i1].z-rp.z, 2) + Math.Pow(points[i1].y-rp.y, 2));
                        if (points[i1].z-rp.z < 0)
                        {
                            double degzy = Math.PI - Math.Asin( Math2.divide(points[i1].y-rp.y , dzy) );

                            int yr = (int)Math.Round((Math.Sin(degzy + rx) * dzy));
                            int zr = (int)Math.Round((Math.Cos(degzy + rx) * dzy));

                            points[i1].y = yr + rp.y;
                            points[i1].z = zr + rp.z;
                        }
                        else
                        {
                            double degzy = Math.Asin(Math2.divide(points[i1].y-rp.y , dzy) );

                            int yr = (int)Math.Round((Math.Sin(degzy + rx) * dzy));
                            int zr = (int)Math.Round((Math.Cos(degzy + rx) * dzy));

                            points[i1].y = yr + rp.y;
                            points[i1].z = zr + rp.z;
                        }
                        break;
                    case "Z":

                        // rotate Z
                        double dxy = Math.Sqrt(Math.Pow(points[i1].x-rp.x, 2) + Math.Pow(points[i1].y-rp.y, 2));
                        if (points[i1].x-rp.x < 0)
                        {
                            double degxy = Math.PI - Math.Asin( Math2.divide(points[i1].y-rp.y , dxy) );

                            int xr = (int)Math.Round((Math.Cos(degxy + rz) * dxy));
                            int yr = (int)Math.Round((Math.Sin(degxy + rz) * dxy));

                            points[i1].x = xr + rp.x;
                            points[i1].y = yr + rp.y;
                        }
                        else
                        {
                            double degxy = Math.Asin( Math2.divide(points[i1].y-rp.y , dxy) );

                            int xr = (int)Math.Round((Math.Cos(degxy + rz) * dxy));
                            int yr = (int)Math.Round((Math.Sin(degxy + rz) * dxy));

                            points[i1].x = xr + rp.x;
                            points[i1].y = yr + rp.y;
                        }
                        break;
                }
                sri++;
            }




            i1++;
        }


        return points;
    }


}



class E
{
    public static String empty = "`";
}
