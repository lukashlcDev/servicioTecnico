namespace ServicioTecnico;

public class Cliente
{
	public int Id { get; set; }

	public string Nombre { get; set; }

	public string Direccion { get; set; }

	public string Documento { get; set; }

	public string Telefono { get; set; }

	public Cliente()
	{
	}

	public Cliente(int id, string nombre, string direccion, string documento, string telefono)
	{
		Id = id;
		Nombre = nombre;
		Direccion = direccion;
		Documento = documento;
		Telefono = telefono;
	}
}
