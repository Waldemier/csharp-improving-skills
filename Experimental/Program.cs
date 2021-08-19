// Командная строка для компиляции выглядит следующим образом:
// csc Test.cs /r:FirstAlias=First.dll /r:SecondAlias=Second.dll
//extern alias FirstAlias; // Визначення зовнішнього псевдоніма
//extern alias SecondAlias;

//using FD = FirstAlias::Demo; // Ссылка на внешний псевдоним с помощью псевдонима пространства имен

using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Reactive.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Experimental.Partials;
using Experimental.Utils;
using MediatR;

namespace Experimental
{
    delegate T AnimalExampleDelegate<T>(T x);

    static class Program
    {
        static async Task Main(string[] args)
        {
            #region Nullable types
            
            Nullable<int> n = new Nullable<int>();
            Console.WriteLine(n.GetValueOrDefault(10)); // set 10 for default
            Console.WriteLine(n.Equals(null)); // True
            
            Nullable<int> five = 5;
            object boxed = five; // boxed in System .Int32 (not System.Nullable<System.Int32>)
            int unboxed = (int)boxed;

            var what = Nullable.GetUnderlyingType(typeof(Nullable<int>)); // System.Int32
            Console.WriteLine(what);

            int? six = 6;
            object boxedSix = six;
            int normalSix = (int) six;
            int? unboxedSix = (int?) six;
            Console.WriteLine(unboxedSix.HasValue);
            six = new int?(); // allowed syntax
           // Console.WriteLine(six.Value); // exception (because null)
           
            #endregion

            Bear bear = new Bear();

            Action act = delegate
            {
                Console.WriteLine("Delegate without parameters");
                var x = six; // Захвачена зовнішня змінна
            };

            act.Invoke();

            #region Iteration block

            IEnumerable<int> CreateEnumerable(DateTime time) 
            {
                Console.WriteLine("Starting"); 
                for (int i = 0; i <= 100; i++)
                {
                    Console.WriteLine("Starting in loop {0}", i);
                    try
                    {
                        if (DateTime.Now >= time)
                        {
                            Console.WriteLine("Iterates breaking");
                            yield break;
                        }

                        yield return i; 
                        yield return 2;
                    }
                    finally
                    {
                        Console.WriteLine("Finally {0}", i);
                    }
                }

                Console.WriteLine("End of iterated block");
            }

            IEnumerable<int> enumerable = CreateEnumerable(DateTime.Now.AddSeconds(1));
            using var enumerator = enumerable.GetEnumerator();

            Console.WriteLine("Default: {0}", enumerator.Current); // 0, оскільки 0 - дефолтне значення для int
            
            while (enumerator.MoveNext())
            {
                // Current: 0
                // Current: 2
                // Finally
                // Current: 1
                // Current: 2
                // Finally
                // Iterates breaking
                // Finally
            
                Console.WriteLine("Current: {0}", enumerator.Current);
                Thread.Sleep(300);
            }

            IEnumerator<int> GetEnumeratorCheckFinally()
            {
                try
                {
                    for (int i = 0; i < 3; i++)
                    {
                        yield return i;
                    }
                }
                finally
                {
                    Console.WriteLine("Finally");
                }
            }

            var getenumMethod = GetEnumeratorCheckFinally();
            getenumMethod.MoveNext();
            getenumMethod.MoveNext();
            getenumMethod.MoveNext();
            //getenumMethod.MoveNext(); // видасть блок finally, оскільки 4 елементу немає.
            getenumMethod.Dispose(); // Провокує блок finally.

            #endregion
            
            # region ^ operator
            
            Car<string,string> car = new Car<string,string>();
            Console.WriteLine(car.Value); // 7

            uint a = 4; // 	100
            uint b = 5;	//	101

            uint c = a ^ b;

            Console.WriteLine(Convert.ToString(c, toBase: 2)); // 001
            
            #endregion

            // Console.WriteLine(int.MaxValue + 1);

            Bear rf = new Bear();
            Animal.JJJ();
            Animal.Name = "gre";

            // Console.WriteLine((+("geg")).GetType()); // compiler error

            var proper = Animal.Name;
            
            var anon = new Action<int>(delegate(int x) { });

            CustomIterator cc = new CustomIterator() { B = { Value = 5 } };

            void MyMethod(string[] names)
            {
                Console.WriteLine();
            }
            
            MyMethod(new[] { "remkgl", "gerge" });

            object anontype = new { Name = "Www" }; // or var

            object obj = new object();
            object obj2 = obj;
            Console.WriteLine(object.ReferenceEquals(obj, obj2));


            Fruit f = new Fruit();

            int x = 4;
            Unit UnitMethod(ref int x)
            {
                return Unit.Value; // from Mediatr package. It represents a void type
            }

            Console.WriteLine(UnitMethod(ref x));

            var anontype2 = new {Name = "Jeorge"};
            // anontype2.Name = "Vasya"; // compile error
            var anontype3 = new { anontype2.Name, Age = 20 }; // Name, Age fields
            Console.WriteLine(anontype3.Name);
            
            List<Person> family = new List<Person>
            {
                new Person { Name = "Holly", Age = 36 },
                new Person { Name = "Jon", Age = 36 },
                new Person { Name = "Tom", Age = 9 },
                new Person { Name = "Robin", Age = 6 },
                new Person { Name = "William", Age = 6 }
            };

            var converted = family.ConvertAll(x => 
                new { x.Name, IsAdult = x.Age >= 18 });

            Action<Person> print = p => Console.WriteLine($"{p.Name}, {p.Age}");
            family.ForEach(print); 
            family.Sort((x, y) => x.Age.CompareTo(y.Age));
            family.ForEach(print);

            #region LinkedList

            LinkedList<int> linkedList = new LinkedList<int>();
            linkedList.AddFirst(1);
            linkedList.AddLast(2);
            linkedList.AddLast(3);
            linkedList.AddBefore(linkedList.Find(3), 7);
            
            LinkedListNode<int> node1 = linkedList.Last;
            linkedList.AddAfter(node1, 8);
            
            foreach (var node in linkedList)
            {
                Console.WriteLine(node);
            }

            Console.WriteLine(linkedList.Find(8).Previous.Value); // 3
            
            #endregion

            #region Expressions 

            Expression firstArg = Expression.Constant(2);
            Expression secondArg = Expression.Constant(3);
            Expression add = Expression.Add(firstArg, secondArg); // (2 + 3) - вивід на консоль

            var compiled = Expression.Lambda<Func<int>>(add);
            Console.WriteLine(add); // () => (2 + 3)
            Console.WriteLine((compiled.Compile())()); // 5

            void FuncLambda(Expression<Func<int>> d) // () => 0703
            {
                Console.WriteLine((d.Compile())()); // 703
            }
            
            FuncLambda(() => 0703);

            
            List<Person> list = new()
            {
                new Person {  Name = "Nelson", Age = 32 },
                new Person { Name = "Nana", Age = 29 }
            };
            
            Func<Person, bool> filtered = p => p.Name.Equals("Nana");
            list.Where(filtered).ToList().ForEach(e => Console.WriteLine(e.Name)); // 2, 4
            
            #endregion

            
            async Task AsyncMethod()
            {
                Console.WriteLine("Start");
                await Task.Run(HelperMethod);
                Console.WriteLine("End");
            }

            void HelperMethod()
            {
                Console.WriteLine("Before while loop");
                int i = 0;
                while (true)
                {
                    i++;
                    if (i <= 10000000000) break;
                }
                Console.WriteLine("After while loop");
            }
            
            var taskFromMethod = AsyncMethod();

            Console.WriteLine("Main thread before wait.");

            taskFromMethod.Wait();

            Console.WriteLine("Main thread after wait.");

            #region Simple extension method

            Person p = new Person("Vika", 20) ;
            p.PrintName();

            string name = null;
            if (name.IsNullOrEmpty())
            {
                Console.WriteLine("NULL");
            }

            #endregion


            void EmptyMethodRef(ref int x)
            {
                Console.WriteLine(x);
            }

            int xx = 10; 
            EmptyMethodRef(ref xx);

            short sh = 10;
            int integ = (int) sh;
            
            
            decimal ddec = 5m;
            double dd = (double)ddec;
            
            string strNum = "777";
            Console.WriteLine(int.Parse(strNum).GetType()); // System.Int32

            
            ArrayList arrList = new ArrayList() { 1, 2, "Text", 4 };
            IEnumerable<int> castedList = arrList.Cast<int>().Reverse();
            
            // Якщо застосовувати Reverse в попередньому випадку - відбудеться буферезація за спроба реверсу всіх даних,
            // що приведе до винятку одразу на перші ітерації.
            // Якщо ж метод Reverse забрати, то генерація винятку відбудеться тільки на 3 ітерації,
            // оскільки Cast та OfType методи перетворюють елементи по мірі їхнього вилучення 
            // foreach (var cl in castedList) 
            // {
            //     Console.WriteLine(cl);
            // }

            List<string> SampleData = new List<string>()
            {
                "Nana",
                "Nelson",
                "Vika"
            };

            var range = Enumerable.Range(11, 1);

            foreach (var r in range)
            {
                Console.WriteLine(r); // 11
            }

            #region params 

            void PrintIntegers(params int[] elements)
            {
                var enumerator = elements.GetEnumerator();

                while (enumerator.MoveNext())
                    Console.WriteLine(enumerator.Current);        
            }
            
            PrintIntegers(1, 2, 3, 4, 5, 6, 7); 

            #endregion
            
            #region Group By and other

            Console.WriteLine("RANGE % GROUPING");
            
            IEnumerable<int> numbers = Enumerable.Range(0, 10);
            int[] numbers2 = {1,2,3,4,5};
            
            var query = from number in numbers
                group number by number % 3; // Записує всі елементи, які діляться з остачою на 3,
                                            // та видають остачу заданого елементу (number) в групу до цього числа.
                                            // Наприклад якщо число number у нас 0, то 3 % 3 == 0, виходить 3 запишеться в групу числа 0.

            foreach (var group in query)
            {
                //Console.WriteLine(group.Key);
                foreach (var item in group)
                {
                    Console.WriteLine("KEY: {0} , NUMBER: {1}", group.Key, item);
                }
            }

            void ChangeArrayCheck(int[] arr)
            {
                arr[0] = 888;
            }
            
            ChangeArrayCheck(numbers2);

            foreach (var item in numbers2)
            {
                Console.WriteLine(item); // first elem is 888
            }
            
            #endregion

            #region Parallel & PLINQ

            Console.WriteLine("Parallel library");
            Parallel.ForEach(numbers2, new Action<int>(x => Console.WriteLine(x))); // 4 2 3 5 888

            Console.WriteLine("PLINQ");
            var numbersByPlinq = numbers2.AsParallel() // parallel executing
                .Where(x => x % 2 == 0)
                .ToList();

            foreach (var np in numbersByPlinq)
            {
                Console.WriteLine(np);
            }
            
           #endregion
           
           #region Rx.NET (System.Reactive package)

           Console.WriteLine("Rx.NET");
           
           IEnumerable<int> numbers3 = Enumerable.Range(0, 10);
           IObservable<int> observable = numbers3.ToObservable();

           // Subscribe
           // Creating the subscriber,
           // Implementing delegates, which will handle the items
           IDisposable subscription = observable.Subscribe(
               item => Console.WriteLine(item), // handle each item
               exception => Console.WriteLine($"Exception: {exception.Message}"), // handle exceptions
               () => Console.WriteLine("Chain is finished") // handle, when collection is ended
               );
           
           // Unsubscribe
           subscription.Dispose(); 
           
           #endregion

           #region async testing

           Console.WriteLine("Async testing");
           
           async Task<int> LoopAsync()
           {
               int a = 0;

               Console.WriteLine("#{0} FIRST IN METHOD", Thread.CurrentThread.ManagedThreadId); // 1

               Task.Run(() =>
               {
                   Console.WriteLine("#{0} IN LOOP", Thread.CurrentThread.ManagedThreadId); // 9 
                   for (int i = 0; i < 100_000_000; i++)
                   {
                       a += i;
                   }

                   return a;
               }).GetAwaiter().UnsafeOnCompleted(() => Console.WriteLine("Task is finished"));
                
               Console.WriteLine("#{0} SECOND IN METHOD", Thread.CurrentThread.ManagedThreadId); // 9
               
               Thread.Sleep(3000);
               
               return a;
           }
           
           Console.WriteLine("#{0} BEFORE taskResult variable", Thread.CurrentThread.ManagedThreadId); // 1

           var taskResult = await LoopAsync(); // Блокуємо подальше виконання, оскільки звільняти немає для чого.
                                               // Подальше виконання буде виконане потоком з пулу потоків. 
           
           Console.WriteLine("#{0} AFTER taskResult variable", Thread.CurrentThread.ManagedThreadId); // 1

           Console.WriteLine();
           Console.WriteLine();
           
           int result = taskResult;
           
           Console.WriteLine("#{0} AFTER await taskResult variable", Thread.CurrentThread.ManagedThreadId); // 9

           Console.WriteLine("Result: {0}", result);


           int result2 = await LoopAsync();
           
           Console.WriteLine();
           
           Console.WriteLine("#{0} SECOND loop executing", Thread.CurrentThread.ManagedThreadId); // 5

           using (var client = new HttpClient())
           {
               var task2 = client.GetStringAsync("https://google.com");
           
               Console.WriteLine("#{0} AFTER client http request", Thread.CurrentThread.ManagedThreadId); // 12 
               
               var taskResultGoogle = await task2;
               
               Console.WriteLine("#{0} AFTER client http request", Thread.CurrentThread.ManagedThreadId); // 12 
           }

           
           Console.WriteLine("#{0} AFTER using block", Thread.CurrentThread.ManagedThreadId); // 12 

           #endregion

           
           #region records

           var car2 = new Car("Audi", 50, 2018);

           var car3 = car2 with
           {   
               Capacity = 120
           };
           
           Console.WriteLine("CAR REFFERENSES EQUALITY: {0}", car2.Equals(car3));
           
           #endregion

           #region using with enumerator

           var list2 = new List<int>() { 1,2,3,4,5 };
           using var enumerator2 = list2.GetEnumerator(); // using is important
           
           while(enumerator2.MoveNext()) Console.WriteLine(enumerator2.Current); 

           #endregion

           #region IEquatable interface 

           Device d = new Device("Kettle", 30);
           object d2 = d;

           Console.WriteLine(d.Equals(d2)); 

           #endregion

           
           var ppp = new Plain();

           Animal aaaaa = new Bear();

           DateTime? dt = null; // because type is struct

           var objj = new object();
           

           #region Іменовані аргументи та дефолтні значення параметрів 

           void SampleMethodWithDefaultParams(int x = 10, int y = 20)
           {
               Console.WriteLine($"{x}, {y}");
           }
           
           SampleMethodWithDefaultParams(y: 40); // Застосування іменованого аргументу + дефолтного значення параметру x 

           #endregion
           
           #region Work with COM (Word)
           
           // Application арр = new Application { Visible = true }; 
           // app.Documents.Add();
           // Document doc = app.ActiveDocument;
           // Paragraph para = doc.Paragraphs.Add();
           // para.Range.Text = "Thank goodness for C# 4";
           // doc.SaveAs(FileName: "test.doc", Аргументы, переданные по значению
           // FileFormat: WdSaveFormat.wdFormatDocument97);
           // doc.Close();
           // app.Application.Quit(); 
           
           #endregion

           #region is & as operators 

           object objAnimal = new Bear() { Name = "Grizli" };

           if (objAnimal is Bear castToBear)
           {
               Console.WriteLine("Bear: {0}, Type: {1}", castToBear.Name, castToBear.GetType());
           }
            
           Bear castToBear2 = objAnimal as Bear;

           Console.WriteLine("Bear 2: {0}, Type 2: {1}", castToBear2.Name, castToBear2.GetType());
           
           #endregion
           
           #region Variability

           AnimalExampleDelegate<Animal> sample = animal => new Bear();
           var res = sample(bear);

           Console.WriteLine(res.GetType());
 
           #endregion

           // concationation
           string line = "World" + 7;
           Console.WriteLine(line); // World7

           
           
           
           
           
           #region Delegates & events

           void PrintSometing()
           {
               Console.WriteLine("Something");
           }

           void PrintSomething2()
           {
               Console.WriteLine("Something2");
           }
           
           ClassCounter ccc = new ClassCounter();
           
           // Delegate
           ccc.onCount += PrintSometing;
           ccc.onCount += PrintSomething2;
           ccc.onCount += delegate // // Cant be delete
           {
               Console.WriteLine("Something3");
           };

           Animal an = new Bear();
           ccc.onCount += an.SomeMethodForDelegateExample; // Method from another class
           
           ccc.onCount?.Invoke(); // Can be possible
           
           ccc.onCount = null;
           
           ccc.Counter(); 

           // Event
           ccc.OnCountEvent += PrintSometing;
           ccc.OnCountEvent += PrintSomething2;
           ccc.OnCountEvent += delegate // Cant be delete
           {
               Console.WriteLine("Something3");
           };

           // ccc.OnCountEvent = PrintSometing; // Cant be possible
           // ccc.OnCountEvent = null; // Cant be possible
           // ccc.OnCountEvent.Invoke(); // error, cant be possible (only in class)
           
           ccc.OnCountEvent -= PrintSometing;
           ccc.onCountEvent2 += PrintSometing;

           ccc.Counter();

           // ccc.onCountEvent2 += ccc.OnCountEvent; // Not allowed (only in class)

           
           // Delegates concationation

           ClassCounter.MethodContainer mc;
           mc = PrintSometing;
           mc += PrintSomething2;
           
           ClassCounter.MethodContainer mc2;
           mc2 = PrintSometing;

           ClassCounter.MethodContainer? mc3 = (ClassCounter.MethodContainer?) ClassCounter.MethodContainer.Combine(mc, mc2);
           // OR
           ClassCounter.MethodContainer mc4 = mc + mc2;
           Console.WriteLine("Concat");
           mc3.Invoke();

           mc3 += delegate
           {
               Console.WriteLine("Some"); // Can be possible
           };

           
           #endregion

           
           
           
           void PrintNumber(int x)
           {
               Console.WriteLine(x);
           }
          
           Action<int> accc1 = delegate(int x)
           {
               Console.WriteLine(x);
           };
           
           Action<int> accc2 = x => Console.WriteLine(x);
           
           accc2 += PrintNumber;
           
           accc2.Invoke(7); // 7 7
           
           
           
           
           #region Python

           // static Configuration LoadConfiguration()
           // {
           //     ScriptEngine engine = Python.CreateEngine();
           //     ScriptScope scope = engine.CreateScope();
           //     engine.ExecuteFile("configuration.py", scope);
           //     return Configuration.FromScriptScope(scope);
           // }
           

           #endregion

           #region dynamic

           void DynamicPrint(object ob)
           {
               Console.WriteLine("In Dynamic");
           }

           dynamic dy = "something text";
           DynamicPrint(dy); // can be possible
           string text = (string)dy;
           Console.WriteLine(text);

           
           int DynamicMethodWithDelegate(dynamic d)
           {
               return d(7);
           }
           
           dynamic dy2 = new Func<int,int>(x => x * 2);
           int resFromMethod = DynamicMethodWithDelegate(dy2);
           Console.WriteLine(resFromMethod);

           dy2 = "Now that is a string"; // Can be possible (but not with var keyword) 

           #endregion
           
           
           
           
           
           
           #region Reflection & Expression trees
           
           // ========= Equivalent is call a bear instance method =======
           
           MethodInfo bearMethodPrint = typeof(Bear).GetMethod("PrintBear",
               BindingFlags.Instance | BindingFlags.NonPublic);
           
           Console.WriteLine(bearMethodPrint); // Void PrintBear()
           
           var instance = Expression.New(typeof(Bear));
           var callExpression = Expression.Call(instance, bearMethodPrint, null); // new Bear().PrintBear()
           var lambda1 = Expression.Lambda(callExpression); // () => new Bear().PrintBear()
           var compiledLambda = lambda1.Compile(); // System.Action
           compiledLambda.DynamicInvoke(); // "Called from expression tree"

           
           // =============== Equivalent is 4 + 7 and print it in the console ===============

           var body = Expression.Add(Expression.Constant(4), Expression.Constant(7)); // (4 + 7)
           var lambda2 = Expression.Lambda(body, false, null); // () => (4 + 7)
           var lamRes = lambda2.Compile().DynamicInvoke();
           Console.WriteLine(lamRes); // 11
           
           // ============== Equivalent is just person.Name call ===========

           var person = new Person() { Name = "Vika", Age = 20 };

           var parameter = Expression.Parameter(typeof(Person), "person");
           var body2 = Expression.PropertyOrField(parameter, "Name");
           var lambda3 = Expression.Lambda(body2, false, parameter); // person => person.Name
           Console.WriteLine(lambda3); 
           var propValue = lambda3.Compile(); // Func<Person, string>
           Console.WriteLine(propValue);
           
           Console.WriteLine(propValue.DynamicInvoke(person)); // Vika
           
           // ===============================
           
           string text1 = "Vika";
           var condition = !string.IsNullOrWhiteSpace(text1) 
               ? "Greetings, " + text1 
               : null;
           
           // ==== Еквівалент виразу вище ====

           var parameterForMethod = Expression.Parameter(typeof(string), "personName");
           
           var method = typeof(string).GetMethod("IsNullOrWhiteSpace", new Type[] { typeof(string) });

           var call = Expression.Call(method, parameterForMethod);
           
           var callWithNot = Expression.Not(call);
           
           var concatFromStringType = typeof(string).GetMethod(nameof(string.Concat), new Type[] { typeof(string), typeof(string) });
           
           var trueCondition = Expression.Call(concatFromStringType, 
               new Expression[] { Expression.Constant("Greetings, "), parameterForMethod });

           var falseCondition = Expression.Constant(null, typeof(string));

           var conditionalExpression = Expression.Condition(callWithNot, trueCondition, falseCondition);

           var lambda4 = Expression.Lambda<Func<string, string?>>(conditionalExpression, false, parameterForMethod);

           Console.WriteLine(lambda4); // personName => IIF(Not(IsNullOrWhiteSpace(personName)), Concat("Greetings, ", personName), null)

           var compiledLambda2 = lambda4.Compile();

           Console.WriteLine(compiledLambda2);

           var resultFromCompiledLambda = compiledLambda2.DynamicInvoke(text1);

           Console.WriteLine(resultFromCompiledLambda); // Greetings, Vika

           // ==============

           var rsample = 3 * 2 / 2;

           var const1 = Expression.Constant(3, typeof(int));
           var const2 = Expression.Constant(2, typeof(int));
           var const3 = Expression.Constant(2, typeof(int));

           var multiply = Expression.Multiply(const1, const2);
           var divide = Expression.Divide(multiply, const3);

           var lambda5 = Expression.Lambda<Func<int>>(divide, false, null);

           Console.WriteLine(lambda5.Compile().Invoke());

           // =================

           Func<int, int> funcDelegate = x => x + 2; // System.Func`2[System.Int32,System.Int32]

           Console.WriteLine(funcDelegate);

           Expression<Func<int,int>> funcDelegExpr = x => x + 2; // x => (x + 2)

           Console.WriteLine(funcDelegExpr);

           #endregion


           
           
           #region static field

           Console.WriteLine(Bear.Health); // 400
           
           Bear.Health = 500f;

           Console.WriteLine(Bear.Health); // 500

           Bear bbbbb = new Bear();

           Console.WriteLine(Bear.Health); // 400

           #endregion


           // T ThreeFourths<T>(T x) where T: struct => 3 * T / 4; // Error

           #region EventHandler delegate

           void BearEventHandlerInstanceMethod(object sender, EventArgs e)
           {
               var owner = sender as Bear;
               Console.WriteLine($"Bear Name: {owner?.Name}. EventArgs: {e}, {e.GetType()}");
           }
           
           Bear bearWithEvent = new Bear();
           bearWithEvent.NameInserted += BearEventHandlerInstanceMethod;
           bearWithEvent.Name = "Grizli"; 

           #endregion

           
           
           
           #region Equals

           // ======== Value types =========
           
           int testInt = 123;
           double testDouble = 123;

           Console.WriteLine(testInt.Equals(testDouble)); // False
           Console.WriteLine(testInt == testDouble); // True
           
           // ======== Classes =========
           
           Bear testBear = new Bear();
           Bear testBear2 = testBear;
           Animal testAnimal = testBear;

           Console.WriteLine(testBear.Equals(testBear2)); // True
           Console.WriteLine(testBear.Equals(testAnimal)); // True 

           // ======== Structs =========
           
           First testStruct = new First() { X = 5, Y = 5 };
           First testStruct2 = new First() { X = 5, Y = 5 };
           First testStruct3 = testStruct2;

           Console.WriteLine(testStruct.Equals(testStruct2)); // True
           Console.WriteLine(testStruct2.Equals(testStruct3)); // True

           Console.WriteLine("Struct equals testing");
           
           First testStruct4 = new First() { X = 7, Y = 7, B = new Bear() {Name = "White bear"}};
           First testStruct5 = new First() { X = 7, Y = 7, B = new Bear() {Name = "White bear"}};
           First testStruct6 = testStruct5;

           Console.WriteLine(testStruct4.Equals(testStruct5)); // False
           Console.WriteLine(testStruct6.Equals(testStruct5)); // True

           #endregion

           #region Parse and Convert

           IFormatProvider formatter = new NumberFormatInfo(){ NumberDecimalSeparator = "." };
           double testDoubleParse = double.Parse("22.50", formatter);
           Console.WriteLine(testDoubleParse.GetType());
           
           int convertIntTest = 5;
           double convertedToDouble = Convert.ToDouble(convertIntTest);
           Console.WriteLine(convertedToDouble.GetType());

           #endregion

           #region IComparable<T> and IComparer<T>

           First firstInstance = new First() { X = 5, Y = 7 };
           First secondInstance = new First() { X = 5, Y = 5 };
           
           
           First[] enumerableFirsts = new First[] { firstInstance, secondInstance };
            
           Array.Sort(enumerableFirsts, new FirstComparerAsc());
           
           foreach (var ef in enumerableFirsts)
           {
               Console.WriteLine("{0}", ef.Y); // 5, 7 - ASC
           }
           
           Array.Sort(enumerableFirsts, new ComparerStructForFirst());
           
           foreach (var ef in enumerableFirsts)
           {
               Console.WriteLine("{0}", ef.Y); // 7, 5 - DESC
           }

           var newFirstCollection = enumerableFirsts.OrderBy(f => f, new FirstComparerDesc()).ToList();
           
           foreach (var nfC in newFirstCollection)
           {
               Console.WriteLine("{0}", nfC.Y); // 7, 5 - DESC
           }

           // newFirstCollection.Sort(new FirstComparerDesc()); // Sort не повертає нову колекцію, а сортує поточну.

           Console.WriteLine(firstInstance.CompareTo(secondInstance)); // -1, тому що логіка сортування реалізована за спаданням.

           #endregion

           #region Own Indexer

           Custom custom = new Custom(new List<string>() { "Vika", "Nana", "Julia", "Oleh" });
           Console.WriteLine(custom["Vika"]); // Vika
           custom["Nana"] = "Wolodymyr";
           Console.WriteLine(custom["Wolodymyr"]); // Wolodymyr
           
           #endregion

           decimal ddd = 4m;
           int iii = 4;
           Console.WriteLine(ddd * iii);

           async Task AsyncVoidMethod()
           {
               await Task.Factory.StartNew(() => throw new Exception());
           }

           AsyncVoidMethod(); // Exception doesnt throw, because method call without await keyword


           #region Interfaces testing

           ITest test = new TestClass("Test 1");
           ITest testSecond = new TestClassSecond("Test 2");
           
           var cast = (TestClassSecond)testSecond; // or "as"
           cast.GetRandomNumber(); // TestSecondClass own method.           

           #endregion
           
           Func<int, Task<IEnumerable<int>>> funcTask = async delegate(int i)
           {
               return await Task.Factory.StartNew(() => Enumerable.Range(0, 100).ToList().Select(x => x * 2));
           };

           #region ExpandoObject and DynamicObject

           dynamic expando = new ExpandoObject();   
           
           expando.Name = "Vika";
           expando.Age = 18;
           expando.Action = new Action<int>(incr => expando.Age += incr);
           
           Console.WriteLine("{0}, {1}", expando.Name, expando.Name.GetType()); // System.String
           Console.WriteLine("{0}, {1}", expando.Age , expando.Age.GetType()); // System.Int32

           expando.Action(7);
           Console.WriteLine("Vikas age: {0}", expando.Age);
           
           dynamic dynamicObject = new MyObj();
           dynamicObject.Fullname = "Viktoria Bugai";

           Console.WriteLine(dynamicObject.Fullname);
           
           ExpandoObject expando2 = new ExpandoObject();
           expando2.TryAdd("Name", "Viktoria");
           var expandoCastToDict = (IDictionary<string, object>)expando2;
           Console.WriteLine(expandoCastToDict["Name"]);
           
           #endregion
        }
    }

    class MyObj: DynamicObject
    {
        private Dictionary<string, object> members = new();
        
        public override bool TrySetMember(SetMemberBinder binder, object? value)
        {
            if (value == null) return false;
            this.members[binder.Name] = value;
            return true;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object? result)
        {
            result = null;
            if (!members.ContainsKey(binder.Name)) return false;

            result = members[binder.Name];
            return true;
        }
    }
    
    struct BuilderCore
    {
        class MoveNextRunning // cant be protected, cause struct cant be extended
        {
            
        }

        struct Tracer 
        {
            
        }
    }
    
    class BuilderClass
    {
        struct MoveNextRunningStruct
        {
            
        }

        protected class Tracer
        {
            
        }
    }

    abstract class AbstrClass
    {
        public abstract void Print();

        public virtual void Something()
        {
            
        }
    }

    sealed class ChildClass: AbstrClass
    {
        public override void Print()
        {
            throw new NotImplementedException();
        }
    }

    interface IBase
    {
        
    }

    interface IChild: IBase
    {
        
    }
    
    interface ITest
    {
        void Print();
    }

    sealed class TestClassSecond: ITest
    {
        public string Name { get; private set; }
        
        public TestClassSecond(string name)
        {
            this.Name = name;
        }

        public int GetRandomNumber()
        {
            var rnd = new Random();
            return rnd.Next(0, 100);
        }
        
        public void Print()
        {
            Console.WriteLine(this.Name);
        }
    }
    
    sealed class TestClass: ITest
    {
        public string Name { get; private set; }
        
        public TestClass(string name)
        {
            this.Name = name;
        }
        
        public void Print()
        {
            Console.WriteLine(this.Name);
        }
    }
    
    class Custom: List<string>
    {
        public Custom(List<string> list)
        {
            AddRange(list);
        }

        public string this[string itemName]
        {
            get
            {
                if (string.IsNullOrWhiteSpace(itemName)) throw new ArgumentNullException();
                
                foreach (var item in this)
                {
                    if (item == itemName) return item;
                }

                return null;
            }
            set
            {
                int index = this.IndexOf(itemName);
                if (index != -1)
                    this[index] = value;
                else
                    throw new ArgumentException();
            }
        }

        public string this[int i, int j, int k, int l] // Can be possible
        {
            get => this[i];
        }
    }
    
    
    class FirstComparerDesc: IComparer<First>
    {
        public int Compare(First x, First y)
        {
            return -x.Y.CompareTo(y.Y);
        }
    }

    sealed class FirstComparerAsc: IComparer<First>
    {
        public int Compare(First x, First y)
        {
            return x.Y.CompareTo(y.Y);
        }
    }

    struct ComparerStructForFirst : IComparer<First>
    {
        public int Compare(First x, First y)
        {
            return -x.Y.CompareTo(y.Y);
        }
    }

    struct First: IComparable<First>
    {
        public int X { get; init; }
        public int Y { get; init; }
        public Bear B { get; init; }
    
        public int CompareTo(First other)
        {
            return -this.Y.CompareTo(other.Y);
        }
    }

    record VahicleBase(string Name, float Capacity, int Year)
    {
        /// <summary>
        /// By default auto-prop are generated by init modifier.
        /// But you can create own property.
        /// </summary>
        /// <returns></returns>
        public string Name { get; set; }
        public override string ToString()
        {
            return $"Name: {this.Name}, Capacity: {this.Capacity}, Graduation Year: {this.Year}";
        }
    }

    sealed record Car(string Name, float Capacity, int Year) : VahicleBase(Name, Capacity, Year), IDisposable
    {
        public void Dispose()
        {
        }
    }

    public record Plain();
    
    internal class Device: IEquatable<Device>
    {
        /// <summary>
        /// Experimental with implementation IEquatable<T> interface
        /// </summary>
        public string Name { get; private set; }
        public decimal Price { get; private set; }

        public Device(string Name, decimal Price)
        {
            this.Name = Name;
            this.Price = Price;
        }

        public virtual bool Equals(Device other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name == other.Name && Price == other.Price;
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is Device other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Price);
        }
    }

    internal class Person 
    {
        public string Name { get; init; }
        public int Age { get; init; }

        public Person()
        {
        }

        public Person(string name, int age)
        {
            this.Name = name;
            this.Age = age;
        }

        public static bool operator ==(Person p1, Person p2)
        {
            return p1.Name == p2.Name;
        }

        public static bool operator !=(Person p1, Person p2)
        {
            return !(p1 == p2);
        }
    }
}