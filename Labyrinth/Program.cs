using Labyrinth;
using Labyrinth.Random;

var lab = new Labyrinth.Labyrinth("""
                                  +-----------+
                                  |           |
                                  |  +---+    |
                                  |           |
                                  +---+       |
                                              |
                                  +---+   ↑   |
                                  |           |
                                  +-----------+
                                  """);

var explorer = new RandomExplorer(lab.NewCrawler(), new RandomDirectionGenerator())
{
    DelayBetweenMove = 200,
    MaxActionAllowed = 100
};

var screen = new Screen(lab);
screen.DrawLabyrinth();
screen.SubscribeTo(explorer);

explorer.Explore();

screen.Dispose();

Console.WriteLine("Exploration terminée !");