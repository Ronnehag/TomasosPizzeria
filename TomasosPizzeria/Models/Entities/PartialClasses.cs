using Microsoft.AspNetCore.Mvc;

namespace TomasosPizzeria.Models.Entities
{
    // These classes are only for mapping the validation from the metadata to the partial classes generated
    // From entity framework.

    [ModelMetadataType(typeof(KundMetaData))]
    public partial class Kund
    {

    }

    [ModelMetadataType(typeof(DishMetaData))]
    public partial class Matratt
    {

    }

    [ModelMetadataType(typeof(ProduktMetaData))]
    public partial class Produkt
    {

    }
}
