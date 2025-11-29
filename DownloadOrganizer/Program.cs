// to make file organizer
// I need to categorize the type of file that i want to organize
// And need to make those folder with category
// After downloading the files, i have to move these file to the respective oranized folder.
// Need to run the console using windows taks scheduler and cron job
  

var config = FileOrganizer.Load("Config.json");
config.Oragnize();
 Console.WriteLine($"[{DateTime.Now}] Downloads cleaned successfully.");





