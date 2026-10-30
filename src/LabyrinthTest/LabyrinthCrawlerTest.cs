using Labyrinth.Build;
using Labyrinth.Crawl;
using Labyrinth.Tiles;
using Labyrinth.Items;

namespace LabyrinthTest.Crawl;

[TestFixture(Description = "Integration test for the crawler implementation in the labyrinth")]
public class LabyrinthCrawlerTest
{

    private static ICrawler newCrawlerFor(string asciiMap)
    {
        var laby = new Labyrinth.Labyrinth(new AsciiParser(asciiMap));
        return laby.NewCrawler();
    }

    [Test]
    public void TurningOneCrawlerDoesNotTurnAnotherOnDifferentLabyrinths()
    {
        var crawler1 = newCrawlerFor("""
            +--+
            |x |
            +--+
            """);
        
        var crawler2 = newCrawlerFor("""
            +--+
            | x|
            +--+
            """);
        
        Assert.That(crawler1.Direction, Is.EqualTo(Direction.North));
        Assert.That(crawler2.Direction, Is.EqualTo(Direction.North));

        crawler1.Direction.TurnLeft();

        Assert.That(crawler1.Direction, Is.EqualTo(Direction.West));
        Assert.That(crawler2.Direction, Is.EqualTo(Direction.North));
    }

    [Test]
    public void TurningOneCrawlerDoesNotTurnAnotherOnSameLabyrinth()
    {
        var laby = new Labyrinth.Labyrinth(new AsciiParser("""
            +--+
            |x |
            +--+
            """));
        var crawler1 = laby.NewCrawler();
        var crawler2 = laby.NewCrawler();

        crawler1.Direction.TurnLeft();

        using var all = Assert.EnterMultipleScope();
        Assert.That(crawler1.Direction, Is.EqualTo(Direction.West));
        Assert.That(crawler2.Direction, Is.EqualTo(Direction.North));
    }

    #region Initialization
    [Test]
    public void InitWithCenteredX()
    {
        var test = newCrawlerFor("""
            +--+
            | x|
            +--+
            """);

        using var all = Assert.EnterMultipleScope();

        Assert.That(test.X, Is.EqualTo(2));
        Assert.That(test.Y, Is.EqualTo(1));
        Assert.That(test.Direction, Is.EqualTo(Direction.North));
        Assert.That(test.FacingTile, Is.TypeOf<Wall>());
    }

    [Test]
    public void InitWithMultipleXUsesLastOne()
    {
        var test = newCrawlerFor("""
            +--+
            |x |
            | x|
            +--+
            """);

        using var all = Assert.EnterMultipleScope();

        Assert.That(test.X, Is.EqualTo(2));
        Assert.That(test.Y, Is.EqualTo(2));
    }

    [Test]
    public void InitWithNoXThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => newCrawlerFor("""
            +--+
            |  |
            |  |
            +--+
            """));
    }
    #endregion

    #region Labyrinth borders
    [Test]
    public void FacingNorthOnUpperTileReturnsOutside()
    {
        var test = newCrawlerFor("""
            +x-+
            |  |
            +--+
            """);
        Assert.That(test.FacingTile, Is.TypeOf<Outside>());
    }

    [Test]
    public void FacingWestOnFarLeftTileReturnsOutside()
    {
        var test = newCrawlerFor("""
            +--+
            x  |
            +--+
            """);
        test.Direction.TurnLeft();
        Assert.That(test.FacingTile, Is.TypeOf<Outside>());
    }

    [Test]
    public void FacingEastOnFarRightTileReturnsOutside()
    {
        var test = newCrawlerFor("""
            +--+
            |  x
            +--+
            """);
        test.Direction.TurnLeft();
        test.Direction.TurnLeft();
        test.Direction.TurnLeft();      // turning 3 times to face east

        Assert.That(test.FacingTile, Is.TypeOf<Outside>());
    }

    [Test]
    public void FacingSouthOnBottomTileReturnsOutside()
    {
        var test = newCrawlerFor("""
            +--+
            |  |
            +x-+
            """);
        test.Direction.TurnLeft();
        test.Direction.TurnLeft(); // turning 2 times to face south

        Assert.That(test.FacingTile, Is.TypeOf<Outside>());
    }
    #endregion

    #region Moves
    [Test]
    public void TurnLeftFacesWestTile()
    {
        var test = newCrawlerFor("""
            + -+
            |x |
            +--+
            """); // By default, the crawler is facing north so he is facing a room.
        
        Assert.That(test.FacingTile, Is.TypeOf<Room>());

        test.Direction.TurnLeft(); // we turn the crawler 1 time to the left the he will face a wall
        Assert.That(test.FacingTile, Is.TypeOf<Wall>());

        test.Direction.TurnLeft();
        test.Direction.TurnLeft();
        Assert.That(test.FacingTile, Is.TypeOf<Room>());
    }

    [Test]
    public void WalkReturnsInventoryAndChangesPositionAndFacingTile()
    {
        var test = newCrawlerFor("""
            +--+
            |  |
            |x |
            +--+
            """);
        

        var inventory = test.Walk();

        using var all = Assert.EnterMultipleScope();
        Assert.That(test.X, Is.EqualTo(1));
        Assert.That(test.Y, Is.EqualTo(1));
        Assert.That(test.FacingTile, Is.TypeOf<Wall>());
        Assert.That(inventory.HasItem, Is.False);
    }

    [Test]
    public void TurnAndWalkReturnsInventoryChangesPositionAndFacingTile()
    {
        var test = newCrawlerFor("""
            +--+
            |  |
            |x  
            +--+
            """);
        
        test.Direction.TurnRight();
        var inventory = test.Walk();

        using var all = Assert.EnterMultipleScope();
        Assert.That(test.X, Is.EqualTo(2));
        Assert.That(test.Y, Is.EqualTo(2));
        Assert.That(test.FacingTile, Is.TypeOf<Room>());
        Assert.That(inventory.HasItem, Is.False);
    }

    [Test]
    public void WalkOnNonTraversableTileThrowsInvalidOperationException()
    {
        var test = newCrawlerFor("""
            +--+
            |x |
            |  |
            +--+
            """);

        Assert.Throws<InvalidOperationException>(() =>
        {
            test.Walk();
        });
    }
    #endregion

    #region Items and doors
    [Test]
    public void WalkInARoomWithAnItem()
    {
        var test = newCrawlerFor("""
            +--+
            |k |
            |x /
            +--+
            """);
        
        var inventory = test.Walk();
        Assert.That(inventory.HasItem, Is.True);
    }

    [Test]
    public void WalkUseAWrongKeyToOpenADoor()
    {
        var test = newCrawlerFor("""
            +--+
            |k |
            | x/
            +--+
            """);
        
        // generate a fake key
        var fakeInventory = new MyInventory(new Key());

        test.Inventory.MoveItemFrom(fakeInventory);
        test.Direction.TurnRight();

        test.Walk(); // go to the door and try to open it with a fake key
        Assert.That(test.FacingTile, Is.TypeOf<Door>());
        Assert.That(test.X, Is.EqualTo(2));
        Assert.That(test.Y, Is.EqualTo(2));
        Assert.That(test.Inventory.HasItem, Is.True); // the fake key is still in the inventory because it did not open the door
    }

    [Test]
    public void WalkUseKeyToOpenADoorAndPass()
    {
        var laby = new Labyrinth.Labyrinth(new AsciiParser("""
                +--+
                |xk|
                +-/|
                """));
        var test = laby.NewCrawler();

        test.Direction.TurnRight();

        var inventory = test.Walk();

        test.Direction.TurnRight();
        ((Door)test.FacingTile).Open(inventory);

        test.Walk();

        using var all = Assert.EnterMultipleScope();

        Assert.That(test.X, Is.EqualTo(2));
        Assert.That(test.Y, Is.EqualTo(2));
        Assert.That(test.Direction, Is.EqualTo(Direction.South));
        Assert.That(test.FacingTile, Is.TypeOf<Outside>());
    }
    #endregion
}