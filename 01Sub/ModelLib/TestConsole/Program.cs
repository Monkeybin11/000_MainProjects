using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLib.Monad;
using MachineLib.DeviceLib;
using System.IO;

namespace TestConsole
{
	class Student
	{
		public string Name;
		public double Average;
	}

	class Program
	{
		static string path = @"E:\temp\pos.csv";
		static string path2 = @"E:\temp\thckness.csv";
		static void Main( string [ ] args )
		{
			var res = File.ReadAllLines(path);
			var res1 = File.ReadAllText(path);
			var res2 = File.ReadLines(path).ToList();
			Console.WriteLine();
		}

		static void main2()
		{
			test1();
			test2();
			test3();


			Tuple<int,string>[] t1 = new Tuple<int, string>[]
			{
				Tuple.Create( 1 , "1"),
				Tuple.Create( 2 , "11"),
				Tuple.Create( 3 , "111"),
				Tuple.Create( 4 , "1111"),
				Tuple.Create( 5 , "11111")

			};

			List<Tuple<int,string>> t2 = new List<Tuple<int, string>>();
			t2.Add( Tuple.Create( 1 , "2" ) );
			t2.Add( Tuple.Create( 10 , "21" ) );
			t2.Add( Tuple.Create( 3 , "211" ) );
			t2.Add( Tuple.Create( 44 , "2111" ) );
			t2.Add( Tuple.Create( 5 , "21111" ) );

			var result  = t1.GroupJoin(t2,
								c => c.Item1,
								o => o.Item1,
								(c , res) => Tuple.Create(c.Item1,c.Item2,res)).ToList();
		}

		static void test1() {
			List<Student> listStudent = new List<Student>
					{
						new Student() { Name = "김철수", Average = 78.5 },
						new Student() { Name = "김영희", Average = 91.2 },
						new Student() { Name = "홍길동", Average = 77.3 },
						new Student() { Name = "김길수", Average = 80.8 }
					};

			var queryStudent = from student in listStudent
							   orderby student.Average
							   group student by student.Average < 80.0;

			foreach ( var studentGroup in queryStudent )
			{
				Console.WriteLine( studentGroup.Key ? "평균 80점 미만:" : "평균 80점 이상:" );

				foreach ( var student in studentGroup )
					Console.WriteLine( "\t{0}: {1}점" , student.Name , student.Average );
			}

		}

		static void test2()
		{
			List<Student> listStudent = new List<Student>
					{
						new Student() { Name = "김철수", Average = 78.5 },
						new Student() { Name = "김영희", Average = 91.2 },
						new Student() { Name = "홍길동", Average = 77.3 },
						new Student() { Name = "김길수", Average = 80.8 },
						new Student() { Name = "김영순", Average = 54.2 },
						new Student() { Name = "김상수", Average = 90.8 },
						new Student() { Name = "이한수", Average = 61.4 }
					};

			var queryStudent = from student in listStudent
							   group student by (int)student.Average / 10 into g
							   orderby g.Key
							   select g;

			foreach ( var studentGroup in queryStudent )
			{
				int temp = studentGroup.Key * 10;
				Console.WriteLine( "{0}점과 {1}점의 사이:" , temp , temp + 10 );

				foreach ( var student in studentGroup )
					Console.WriteLine( "\t{0}: {1}점" , student.Name , student.Average );
			}



		}

		static void test3()
		{
			List<MyAverage> listAverage = new List<MyAverage>
					{
						new MyAverage() { Name = "김철수", Average = 78.5 },
						new MyAverage() { Name = "김영희", Average = 91.2 },
						new MyAverage() { Name = "홍길동", Average = 77.3 },
						new MyAverage() { Name = "김길수", Average = 80.8 },
						new MyAverage() { Name = "김영순", Average = 54.2 },
						new MyAverage() { Name = "김상수", Average = 90.8 },
						new MyAverage() { Name = "이한수", Average = 61.4 }
					};
					
								List<MyHobby> listHobby = new List<MyHobby>
					{
						new MyHobby() { Name = "김영순", Hobby = "자전거 타기" },
						new MyHobby() { Name = "홍길동", Hobby = "컴퓨터 게임" },
						new MyHobby() { Name = "이한수", Hobby = "피아노 연주" },
						new MyHobby() { Name = "김철수", Hobby = "축구" }
					};


			var queryStudent = from student in listAverage
							   join hobby in listHobby on student.Name equals hobby.Name
							   select new { Name = student.Name, Average = student.Average, Hobby = hobby.Hobby };

			foreach ( var studentGroup in queryStudent )
				Console.WriteLine( "이름: {0}\n\t평균: {1}점\n\t취미: {2}" , studentGroup.Name , studentGroup.Average , studentGroup.Hobby );


		}
	}
}
