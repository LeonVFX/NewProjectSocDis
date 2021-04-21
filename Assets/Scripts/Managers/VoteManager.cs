using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoteManager : MonoBehaviour
{
    public static VoteManager vm;
    private Timer timer;
    public int[] votingResult;
    public bool[] hasVoted;
    private bool everyOneVoted
    {
        get
        {
            if (GameManager.gm.currentStage == GameManager.GameStage.Voting)
            {
                foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
                {
                    if (hasVoted[player.ActorNumber])
                        continue;
                    return false;
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    private bool voteFinished = false;
    private bool resultDraw = false;

    private void Awake()
    {
        // Singleton
        if (vm == null)
        {
            vm = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        GameManager.gm.OnVoteStage += BeginVoting;
        timer = GetComponent<Timer>();
    }

    private void Update()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        if ((everyOneVoted && !voteFinished) || (timer.timeOver))
        {
            int mostVotedPlayer = EndVoting();
            if (!resultDraw)
            {
                // END GAME IF CREATURE IS VOTED OUT
                if (PlayerManager.pm.IsCreature(mostVotedPlayer))
                    EndManager.em.CreatureVoted();
                else
                    PlayerManager.pm.KillPlayer(mostVotedPlayer);
            }
            voteFinished = true;
            GameManager.gm.NextStage();
        }
    }

    private void BeginVoting()
    {
        votingResult = new int[PhotonNetwork.CurrentRoom.PlayerCount + 1];
        hasVoted = new bool[PhotonNetwork.CurrentRoom.PlayerCount + 1];
        hasVoted[0] = true;
    }

    private int EndVoting()
    {
        int mostVotes = 0;
        int mostVotesIndex = 0;

        for (int i = 1; i < votingResult.Length; ++i)
        {
            if (votingResult[i] > mostVotes)
            {
                mostVotes = votingResult[i];
                mostVotesIndex = i;
                resultDraw = false;
            }
            // In case of draw
            else if (votingResult[i] == mostVotes)
            {
                resultDraw = true;
            }
        }
        return mostVotesIndex;
    }

    [PunRPC]
    private void RPC_Vote(string playerTagName, int playerTagNumber, int playerNumber)
    {
        votingResult[playerTagNumber]++;
        hasVoted[playerNumber] = true;
        Debug.Log($"Player {playerTagNumber.ToString()}: {playerTagName}, has {votingResult[playerTagNumber]} votes");
    }
}