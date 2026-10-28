using Labyrinth.Build;
using Labyrinth.Crawl;
using Labyrinth.Tiles;

namespace LabyrinthTest.Crawl;

[TestFixture(Description = "Integration test for the crawler implementation in the labyrinth")]
public class LabyrinthCrawlerTest
{

    private static ICrawler newCrawlerFor(string asciiMap)
    {
        var laby = new Labyrinth.Labyrinth(new AsciiParser(asciiMap));
        return laby.NewCrawler();
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
        Assert.That(false);
    }
    #endregion

    #region Labyrinth borders
    [Test]
    public void FacingNorthOnUpperTileReturnsOutside()
    {
        Assert.That(false);
    }

    [Test]
    public void FacingWestOnFarLeftTileReturnsOutside()
    {
        Assert.That(false);
    }

    [Test]
    public void FacingEastOnFarRightTileReturnsOutside()
    {
        Assert.That(false);
    }

    [Test]
    public void FacingSouthOnBottomTileReturnsOutside()
    {
        Assert.That(false);
    }
    #endregion

    #region Moves
    [Test]
    public void TurnLeftFacesWestTile()
    {
        Assert.That(false);
    }

    [Test]
    public void WalkReturnsInventoryAndChangesPositionAndFacingTile()
    {
        Assert.That(false);
    }

    [Test]
    public void TurnAndWalkReturnsInventoryChangesPositionAndFacingTile()
    {
        Assert.That(false);
    }

    [Test]
    public void WalkOnNonTraversableTileThrowsInvalidOperationException()
    {
        Assert.That(false);
    }
    #endregion

    #region Items and doors
    [Test]
    public void WalkInARoomWithAnItem()
    {
        Assert.That(false);
    }

    [Test]
    public void WalkUseAWrongKeyToOpenADoor()
    {
        Assert.That(false);
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