namespace PrimeiraApi.Model
{
    public interface IEmployeeRepository
    {
        void Add(Employee employee);

        List<Employee> Get();
        Employee? Get(int id); //Traz somente 1 usuario


    }
}
