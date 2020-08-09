namespace AuthProject.Models
{
    public class ResultVM
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class ResultVM<T> : ResultVM
    {
        public T Data { get; set; }
    }
}
