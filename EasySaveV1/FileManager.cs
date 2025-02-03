namespace EasySave
{
    public class FileManager
    {
        static void ListDirectories(string source)
        {
            try
            {
                foreach (string directory in Directory.GetDirectories(source))
                {
                    Console.WriteLine(directory);
                    ListDirectories(directory); // Appel récursif pour lister les sous-répertoires
                }
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine($"Accès refusé à {source}: {e.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erreur lors de la lecture de {source}: {e.Message}");
            }
        }
    }
}
