using System.Collections;
using System.Collections.Generic;
using UIToolkitDemo;
using UnityEngine;
using Utilities.Inspector;
using System;

public enum CutsceneMode
{
    Play,
    None
}

public class BattleGameplayManager : MonoBehaviour
{
    public static event Action GameWon;
    public static event Action GameLost;

    [Header("Teams")]
    public List<UnitController> heroTeamUnits;
    public List<UnitController> enemyTeamUnits;

    [Header("Team Logic")]
    public bool autoAssignUnitTeamTargets = false;

    //Runtime Battle Logic
    private List<UnitController> aliveHeroUnits;
    private List<UnitController> aliveEnemyUnits;

    [Header("Battle Intro")]
    public CutsceneMode introCutscene;
    public CutsceneTimelineBehaviour introCutsceneBehaviour;

    [Header("Battle Ended - Victory")]
    public CutsceneTimelineBehaviour victoryCutsceneBehaviour;
    public SceneField victoryNextScene;

    [Header("Battle Ended - Defeat")]
    public CutsceneTimelineBehaviour defeatCutsceneBehaviour;
    public SceneField defeatNextScene;

    private SceneField selectedNextScene;




    void Awake()
    {
        SetupTeamUnits();
        StartGameLogic();
    }

    void SetupTeamUnits()
    {
        CreateAliveUnits();

        if (autoAssignUnitTeamTargets)
        {
            AutoAssignUnitTeamTargets();
        }
    }

    void CreateAliveUnits()
    {
        aliveHeroUnits = new List<UnitController>();

        for (int i = 0; i < heroTeamUnits.Count; i++)
        {
            aliveHeroUnits.Add(heroTeamUnits[i]);
            aliveHeroUnits[i].SetAlive();
            aliveHeroUnits[i].UnitDiedEvent += UnitHasDied;
        }

        aliveEnemyUnits = new List<UnitController>();

        for (int i = 0; i < enemyTeamUnits.Count; i++)
        {
            aliveEnemyUnits.Add(enemyTeamUnits[i]);
            aliveEnemyUnits[i].SetAlive();
            aliveEnemyUnits[i].UnitDiedEvent += UnitHasDied;
        }
    }


    void AutoAssignUnitTeamTargets()
    {
        for (int i = 0; i < aliveHeroUnits.Count; i++)
        {
            aliveHeroUnits[i].AssignTargetUnits(aliveEnemyUnits);
        }

        for (int i = 0; i < aliveEnemyUnits.Count; i++)
        {
            aliveEnemyUnits[i].AssignTargetUnits(aliveHeroUnits);
        }
    }

    void StartGameLogic()
    {
        switch (introCutscene)
        {
            case CutsceneMode.Play:
                StartIntroCutscene();
                break;

            case CutsceneMode.None:
                StartBattle();
                break;
        }
    }

    void StartIntroCutscene()
    {
        introCutsceneBehaviour.StartTimeline();
    }

    public void StartBattle()
    {
        for (int i = 0; i < aliveHeroUnits.Count; i++)
        {
            aliveHeroUnits[i].BattleStarted();
        }

        for (int i = 0; i < aliveEnemyUnits.Count; i++)
        {
            aliveEnemyUnits[i].BattleStarted();
        }
    }

    void UnitHasDied(UnitController deadUnit)
    {
        RemoveUnitFromAliveUnits(deadUnit);
    }

    void RemoveUnitFromAliveUnits(UnitController unit)
    {
        CheckRemainingTeams();
        for (int i = 0; i < aliveHeroUnits.Count; i++)
        {
            if (unit == aliveHeroUnits[i])
            {
                aliveHeroUnits[i].UnitDiedEvent -= UnitHasDied;
                aliveHeroUnits.RemoveAt(i);
                RemoveUnitFromEnemyTeamTargets(unit);
            }
        }

        if (aliveHeroUnits.Count > 0)
        {
            for (int i = 0; i < aliveEnemyUnits.Count; i++)
            {
                if (unit == aliveEnemyUnits[i])
                {
                    aliveEnemyUnits[i].UnitDiedEvent -= UnitHasDied;
                    aliveEnemyUnits.RemoveAt(i);
                    RemoveUnitFromHeroTeamTargets(unit);
                }
            }
        }

        CheckRemainingTeams();
    }

    void RemoveUnitFromHeroTeamTargets(UnitController unit)
    {
        for (int i = 0; i < aliveHeroUnits.Count; i++)
        {
            aliveHeroUnits[i].RemoveTargetUnit(unit);
        }
    }

    void RemoveUnitFromEnemyTeamTargets(UnitController unit)
    {
        for (int i = 0; i < aliveEnemyUnits.Count; i++)
        {
            aliveEnemyUnits[i].RemoveTargetUnit(unit);
        }
    }

    void CheckRemainingTeams()
    {

        if (aliveHeroUnits.Count == 0)
        {
            SetBattleDefeat();
            //Debug.Log("Defeat. alive heroes = " + aliveHeroUnits.Count + " alive enemies = " + aliveEnemyUnits.Count);
        }

        else if (aliveEnemyUnits.Count == 0)
        {
            SetBattleVictory();
            //Debug.Log("Victory. alive heroes = " + aliveHeroUnits.Count + " alive enemies = " + aliveEnemyUnits.Count);
        }
    }

    void SetBattleVictory()
    {
        StopAllAliveTeamUnits(aliveHeroUnits);

        if (victoryCutsceneBehaviour != null)
        {
            victoryCutsceneBehaviour.StartTimeline();
        }

        // notify GameScreen to show the Victory UI
        GameWon?.Invoke();

    }

    void SetBattleDefeat()
    {
        StopAllAliveTeamUnits(aliveEnemyUnits);

        if (defeatCutsceneBehaviour != null)
        {
            defeatCutsceneBehaviour.StartTimeline();
        }

        // notify the GameScreen to show the Defeat UI
        GameLost?.Invoke();
    }

    void StopAllAliveTeamUnits(List<UnitController> aliveTeamUnits)
    {
        for (int i = 0; i < aliveTeamUnits.Count; i++)
        {
            aliveTeamUnits[i].BattleEnded();
        }
    }

    public void SelectVictoryNextScene()
    {
        selectedNextScene = victoryNextScene;

    }

    public void SelectDefeatNextScene()
    {
        selectedNextScene = defeatNextScene;
    }

    public void LoadSelectedScene()
    {
        NextSceneLoader sceneLoader = new NextSceneLoader();
        sceneLoader.LoadNextScene(selectedNextScene);
    }

}

