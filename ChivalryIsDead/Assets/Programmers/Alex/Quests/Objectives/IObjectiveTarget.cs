public interface IObjectiveTarget
{
    int ID { get; }
    int Health { get; }
    int MaxHealth { get; }
    bool IsChecked { get; set; }
}
