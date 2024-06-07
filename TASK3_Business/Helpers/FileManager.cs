using Microsoft.AspNetCore.Components.Forms;

namespace Project.Helpers {
  public class FileManager {
    public static string Save(IBrowserFile file, string root, string folder) {
      string newFileName = Guid.NewGuid().ToString() + file.Name;
      string path = Path.Combine(root, folder, newFileName);

      using FileStream fs = new(path, FileMode.Create);
      file.OpenReadStream().CopyTo(fs);

      return newFileName;
    }

    public static bool Delete(string root, string folder, string fileName) {
      string path = Path.Combine(root, folder, fileName);

      if (File.Exists(path)) {
        File.Delete(path);
        return true;
      }
      return false;
    }
  }
}

