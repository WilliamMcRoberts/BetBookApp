using BetBookData.Interfaces;
using BetBookDbAccess;
using BetBookData.Models;
using BetBookData.Helpers;

namespace BetBookData.Data;

public class GameData : IGameData
{
    private readonly ISqlConnection _db;

    /// <summary>
    /// GameData Constructor
    /// </summary>
    /// <param name="db">ISqlConnection represents SqlConnection class interface</param>
    public GameData(ISqlConnection db)
    {
        _db = db;
    }

    /// <summary>
    /// Async method calls the spGames_GetAll stored procedure to retrieve 
    /// all games in the database
    /// </summary>
    /// <returns>
    /// IEnumerable of GameModel representing all games in the database
    /// </returns>
    public async Task<IEnumerable<GameModel>> GetGames()
    {
        return await _db.LoadData<GameModel, dynamic>(
        "dbo.spGames_GetAll", new { });
    }

    /// <summary>
    /// Async method calls spGames_Get stored procedure which retrieves one 
    /// game by game id
    /// </summary>
    /// <param name="id">int represents the Id of a game</param>
    /// <returns>
    /// GameModel represents a game to retrieve from database
    /// </returns>
    public async Task<GameModel?> GetGame(int id)
    {
        var results = await _db.LoadData<GameModel, dynamic>(
            "dbo.spGames_Get", new
            {
                Id = id
            });

        return results.FirstOrDefault();
    }

    /// <summary>
    /// Async method calls the spGames_Insert stored procedure to insert one game 
    /// entry into the database
    /// </summary>
    /// <param name="game">GameModel represents a game to insert into the database</param>
    /// <returns></returns>
    public async Task InsertGame(GameModel game)
    {
        int weekNumber = game.Season.CalculateWeek(game.DateOfGame);
        string seasonType = game.DateOfGame.CalculateSeason().ToString();
        string gameStatus = game.GameStatus.ToString();

        await _db.SaveData("dbo.spGames_Insert", new
        {
            game.HomeTeamId,
            game.AwayTeamId,
            game.FavoriteId,
            game.UnderdogId,
            game.Stadium,
            game.PointSpread,
            weekNumber,
            seasonType,
            game.DateOfGame,
            gameStatus
        });
    }

    /// <summary>
    /// Async method calls the spGames_Update stored procedure to update a game
    /// </summary>
    /// <param name="game">GameModel represents a game to update in the database</param>
    /// <returns></returns>
    public async Task UpdateGame(GameModel game)
    {
        string seasonType = game.Season.ToString();
        string gameStatus = game.GameStatus.ToString();

        if(game.GameWinnerId == 0)
        {
            await _db.SaveData("dbo.spGames_UpdateDraw", new
            {
                game.Id,
                game.HomeTeamId,
                game.AwayTeamId,
                game.FavoriteId,
                game.UnderdogId,
                game.Stadium,
                game.PointSpread,
                game.FavoriteFinalScore,
                game.UnderdogFinalScore,
                seasonType,
                game.DateOfGame,
                gameStatus
            });
        }

        else
        {
            await _db.SaveData("dbo.spGames_Update", new
            {
                game.Id,
                game.HomeTeamId,
                game.AwayTeamId,
                game.FavoriteId,
                game.UnderdogId,
                game.Stadium,
                game.PointSpread,
                game.FavoriteFinalScore,
                game.UnderdogFinalScore,
                game.GameWinnerId,
                seasonType,
                game.DateOfGame,
                gameStatus
            });
        }
    }

    /// <summary>
    /// Async method calls the spGames_Delete stored procedure which deletes one game
    /// entry in the database
    /// </summary>
    /// <param name="id">int represents the Id of a game to delete from database</param>
    /// <returns></returns>
    public async Task DeleteGame(int id)
    {
        await _db.SaveData(
        "dbo.spGames_Delete", new
        {
            Id = id
        });
    }
}




