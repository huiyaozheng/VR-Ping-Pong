using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Manager : MonoBehaviour
{
    public GameState game;
    BotPersonality opponentBot = null; // this field is read during TransitionToGame().

    public GameObject
        parent_mainMenu,
        parent_opponentSelection,
        parent_opponentCreation,
        parent_scoreDisplay,
        parent_credits,
        parent_settings;

    public TextMeshProUGUI
        opponentName0,
        opponentName1,
        opponentName2,
        opponentName3,
        score0,
        score1,
        creation_botName,
        scores_botName;

    void Start()
    {
        parent_mainMenu.SetActive(true);

        parent_credits.SetActive(false);
        parent_opponentCreation.SetActive(false);
        parent_scoreDisplay.SetActive(false);
        parent_credits.SetActive(false);
        parent_settings.SetActive(false);
    }

    public void OnClick_MainMenu_NewGame()
    {
        parent_mainMenu.SetActive(false);
        parent_opponentSelection.SetActive(true);

        BotPersonality bot = null;
        if (BotPersonalityManager.GetBotPersonalities().bots.Count <= 0)
            opponentName0.text = "NULL";
        else
        {
            bot = BotPersonalityManager.GetBotPersonalities().bots[0];
            opponentName0.text = (bot == null) ? "NULL" : bot.botGivenName;
        }

        if (BotPersonalityManager.GetBotPersonalities().bots.Count <= 1)
            opponentName1.text = "NULL";
        else
        {
            bot = BotPersonalityManager.GetBotPersonalities().bots[1];
            opponentName1.text = (bot == null) ? "NULL" : bot.botGivenName;
        }

        if (BotPersonalityManager.GetBotPersonalities().bots.Count <= 2)
            opponentName2.text = "NULL";
        else
        {
            bot = BotPersonalityManager.GetBotPersonalities().bots[2];
            opponentName2.text = (bot == null) ? "NULL" : bot.botGivenName;
        }

        if (BotPersonalityManager.GetBotPersonalities().bots.Count <= 3)
            opponentName3.text = "NULL";
        else
        {
            bot = BotPersonalityManager.GetBotPersonalities().bots[3];
            opponentName3.text = (bot == null) ? "NULL" : bot.botGivenName;
        }
    }

    string GenerateName()
    {
        string[] names = new string[]
        {
            "Adelina", "Annett", "Kristin", "Cherelle", "Vernon", "Ardath", "Ivey", "Lakeisha", "Alejandra", "Verdell",
            "Aisha", "Jettie", "Susan", "Victoria", "Lily", "Sergio", "Haley", "Nola", "Artie", "Sylvia", "Ciara",
            "Sherice", "Brandee", "Chelsey", "Laronda", "Thomasena", "Korey", "Mable", "Benjamin", "Christy", "Albert",
            "Shizuko", "Jeanene", "Karlene", "Rocky", "Fredia", "Maude", "Cole", "Forrest", "Luigi", "Queenie",
            "Earleen", "Charmain", "Lulu", "Ted", "Coleman", "Thersa", "Elina", "Fanny", "Pamala", "Claud", "Tennie",
            "Lenard", "Shin", "Berniece", "Wade", "Treasa", "Ryan", "Stephania", "Kenia", "Denisse", "Moshe", "Frida",
            "Raphael", "Morton", "Bernardine", "Roosevelt", "Zoila", "Susann", "Waylon", "Ken", "William", "Seymour",
            "Dion", "Danyell", "Clark", "Todd", "Maudie", "Tish", "Fatima", "Kera", "Huey", "Kimbra", "Daniela",
            "Thomasina", "Adelle", "Kellie", "Farah", "Kelle", "Linwood", "Ian", "Sally", "Vinnie", "Kaley", "Mickie",
            "Alita", "Jay", "Otelia", "Shaunna", "Manda", "Letha", "Jay", "Arianne", "Athena", "Rufus", "Evalyn",
            "Brandie", "Yasmin", "Vergie", "Asa", "Vincenza", "Jeanetta", "Fleta", "Odis", "Marlys", "Kyung", "Lilly",
            "Bud", "Dot", "Mike", "Yvette", "Natosha", "Danielle", "Ricardo", "Sandy", "Elenor", "Deedra", "Fredrick",
            "Vernia", "Pat", "Lesia", "Christiana", "Gabriella", "Sarai", "Loma", "Donita", "Yong", "Adela", "Ernie",
            "Moriah", "Alyson", "Hanna", "Mila", "Fidelia", "Shea", "Denna", "Catrina", "Eliana", "Bonnie", "Lavon"
        };
        int r = Random.Range(0, names.Length - 1);
        return names[r];
    }

    void TransitionToOpponentCreation()
    {
        parent_opponentSelection.SetActive(false);
        parent_opponentCreation.SetActive(true);
        opponentBot = null; // should be the case anyway, right?

        creation_botName.text = GenerateName();
    }

    public void OnClick_CreationPreset0()
    {
        BotPersonalityManager.CreateBotPersonality(creation_botName.text, BotPersonalityManager.eBotPreset.BP_00);
        opponentBot = BotPersonalityManager.GetBotPersonalities()
            .bots[BotPersonalityManager.GetBotPersonalities().bots.Count - 1];
        TransitionToGame();
    }

    public void OnClick_CreationPreset1()
    {
        BotPersonalityManager.CreateBotPersonality(creation_botName.text, BotPersonalityManager.eBotPreset.BP_01);
        opponentBot = BotPersonalityManager.GetBotPersonalities()
            .bots[BotPersonalityManager.GetBotPersonalities().bots.Count - 1];
        TransitionToGame();
    }

    void TransitionToGame()
    {
        // Start the game!
        parent_opponentSelection.SetActive(false);
        parent_opponentCreation.SetActive(false);
        parent_scoreDisplay.SetActive(true);

        score0.text = "0";
        score1.text = "0";

        scores_botName.text = opponentBot.botGivenName;
        BotPersonalityManager.ChooseOpponent(opponentBot, game);
    }

    public void OnClick_OpponentSelection_Opponent0()
    {
        if (BotPersonalityManager.GetBotPersonalities().bots.Count <= 0)
            TransitionToOpponentCreation();
        else
        {
            opponentBot = BotPersonalityManager.GetBotPersonalities().bots[0];
            if (opponentBot == null)
                TransitionToOpponentCreation();
            else
                TransitionToGame();
        }
    }

    public void OnClick_OpponentSelection_Opponent1()
    {
        if (BotPersonalityManager.GetBotPersonalities().bots.Count <= 1)
            TransitionToOpponentCreation();
        else
        {
            opponentBot = BotPersonalityManager.GetBotPersonalities().bots[1];
            if (opponentBot == null)
                TransitionToOpponentCreation();
            else
                TransitionToGame();
        }
    }

    public void OnClick_OpponentSelection_Opponent2()
    {
        if (BotPersonalityManager.GetBotPersonalities().bots.Count <= 2)
            TransitionToOpponentCreation();
        else
        {
            opponentBot = BotPersonalityManager.GetBotPersonalities().bots[2];
            if (opponentBot == null)
                TransitionToOpponentCreation();
            else
                TransitionToGame();
        }
    }

    public void OnClick_OpponentSelection_Opponent3()
    {
        if (BotPersonalityManager.GetBotPersonalities().bots.Count <= 3)
            TransitionToOpponentCreation();
        else
        {
            opponentBot = BotPersonalityManager.GetBotPersonalities().bots[3];
            if (opponentBot == null)
                TransitionToOpponentCreation();
            else
                TransitionToGame();
        }
    }

    public void OnClick_MainMenu_Settings()
    {
        parent_mainMenu.SetActive(false);
        parent_settings.SetActive(true);
    }

    public void OnClick_Settings_Back()
    {
        parent_mainMenu.SetActive(true);
        parent_settings.SetActive(false);
    }

    public void OnClick_MainMenu_Credits()
    {
        parent_mainMenu.SetActive(false);
        parent_credits.SetActive(true);
    }

    public void OnClick_MainMenu_Exit()
    {
        Application.Quit();
    }

    void OnEvent_rallyEnded()
    {
        if (score0 != null) // Don't remove those.
            score0.text = game.score0.ToString();
        if (score1 != null)
            score1.text = game.score1.ToString();
    }

    void OnEnable()
    {
        Events.rallyEnded += OnEvent_rallyEnded;
    }

    void OnDisable()
    {
        Events.rallyEnded -= OnEvent_rallyEnded;
    }
}