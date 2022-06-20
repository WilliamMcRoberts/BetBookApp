﻿using BetBookDataLogic.Models;

namespace BetBookDataLogic.Data.Interfaces;
public interface ITeamRecordData
{
    Task DeleteTeamRecord(int teamId);
    Task<TeamRecordModel?> GetTeamRecord(int teamId);
    Task<IEnumerable<TeamRecordModel>> GetTeamRecords();
    Task InsertTeamRecord(int teamId);
    Task UpdateTeamRecord(TeamRecordModel teamRecord);
}