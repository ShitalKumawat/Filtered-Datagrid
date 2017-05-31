using System;
using System.Collections.ObjectModel;

namespace TestApplication
{
    public class MainWindowViewModel
    {

        public ObservableCollection<Employee> Employees { get; set; }

        public MainWindowViewModel()
        {
            Employees = new ObservableCollection<Employee>()
             {
               new Employee() {Name = null, Designation =new Status(){stt =false,t1=new ttt() { Id=3 } },Birthdate=DateTime.Now},
               new Employee() {Name = "", Designation =new Status(){stt =true, t1=new ttt() { Id = 4 } },Birthdate=DateTime.Now},
               new Employee() {Name = "Alice", Designation =new Status(){stt =false ,t1=new ttt() {  Id = 3 } },Birthdate=DateTime.Now},
               new Employee() {Name = "John", Designation =new Status(){stt =true,t1=new ttt() {  Id = 4 } },Birthdate =DateTime.Now},
               new Employee() {Name = "Tom1", Designation =new Status(){stt =true ,t1=new ttt() {  Id = 1 } },Birthdate=DateTime.Now},
               new Employee() { Name = "Mary2", Designation = null,Birthdate=DateTime.Now},
               new Employee() {Name = "Alice3", Designation =new Status(){stt =false ,t1=new ttt() {  Id = 3 } },Birthdate=DateTime.Now},
               new Employee() {Name = "John4", Designation =new Status(){stt =true ,t1=new ttt() {  Id = 4} } },
               new Employee() {Name = "Tom5", Designation =new Status(){stt =true ,t1=new ttt() {  Id = 1} } },
               new Employee() {Name = "Mary6", Designation =null},
               new Employee() {Name = "Alice7", Designation =new Status(){stt =false,t1=new ttt() {  Id = 3 } } },
                new Employee() {Name = "John8", Designation =new Status(){stt =true, t1=new ttt() { Id = 4} } }
             };
        }
    }

    public class Employee
    {
        public string Name { get; set; }

        public Status Designation { get; set; }
        
        public DateTime? Birthdate { get; set; }
    }

    public class ttt
    {
        public int Id { get; set; }
    }

    public enum St
    {
        Passed,
        Failed
    }

    public class Status
    {
        public ttt t1 { get; set; }
        public bool? stt { get; set; }

    }
}
