
// Este avance corresponde al de la semana 1 y 2

// Creación de las clases Producto y Cliente
public class Producto
{
    private int id;

    // Método simple para setear el Id
    public void SetId(int nuevoId){
        if(nuevoId  > 0)
        {
            id = nuevoId;
        }
}
}

public class Cliente
{
    private string nombre;

    // Método simple para setear el nombre
    public void SetNombre(string nuevoNombre)
    {
        nuevoNombre = nombre;
    }

}