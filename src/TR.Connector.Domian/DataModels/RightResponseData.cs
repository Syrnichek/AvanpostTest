namespace TR.Connector.Domian.DataModels;

public sealed record RightResponseData(
    int Id,
    string Name,
    object Users //Не совсем понятна логика использования object, в идеале хотел бы реализовать конкретный тип 
);
