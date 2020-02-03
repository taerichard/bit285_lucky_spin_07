using System;
using Microsoft.AspNetCore.Mvc;
using LuckySpin.Models;
using LuckySpin.ViewModels;

namespace LuckySpin.Controllers
{
    public class SpinnerController : Controller
    {
        //TODO: remove reference to the Singleton Repository
        //      and inject a reference (dbcRepo) to the LuckySpinContext 
        private Repository repository;
        Random random = new Random();

        /***
         * Controller Constructor
         */
        public SpinnerController(Repository r)
        {
            repository = r;
        }

        /***
         * Entry Page Action
         **/

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(IndexViewModel info)
        {
            if (!ModelState.IsValid) { return View(); }

            //Create a new Player object
            Player player = new Player
            {
                FirstName = info.FirstName,
                Luck = info.Luck,
                Balance = info.StartingBalance
            };
            //TODO: Update persistent data using dbcRepo.Players.Add() and SaveChanges()
            repository.CurrentPlayer = player;

            //TODO: Pass the player Id to SpinIt
            return RedirectToAction("SpinIt");
        }

        /***
         * Play through one Spin
         **/  
         [HttpGet]      
         public IActionResult SpinIt() //TODO: receive the player Id
        {
            //TODO: Use the dbcRepo.Player.Find() to get the player object

            // QUESTION 1: Why use the repository player information to initialize the SpinItViewModel?
            //            (HINT: See what happens if you don't initialize it.)
            //TODO: Intialize the spinItVM with the player object from the database
            SpinItViewModel spinItVM = new SpinItViewModel() {
                FirstName = repository.CurrentPlayer.FirstName,
                Luck = repository.CurrentPlayer.Luck,
                Balance = repository.CurrentPlayer.Balance
            };

            // QUESTION 2: What else does ChargeSpin() do besides check if there is enough $$ to spin?
            if (!spinItVM.ChargeSpin())
            {
                return RedirectToAction("LuckList");
            }
            // QUESTION 3: Locate the if-else logic to determine a winning spin?
            //             Why do you think it is done there? 
            if (spinItVM.Winner) { spinItVM.CollectWinnings(); }

            // QUESTION 4: Why is it necessary to update the player's balance from the spinItVM after a spin?
            // TODO: Update the player Balance using the Player from the database
            repository.CurrentPlayer.Balance = spinItVM.Balance;

            //Store the Spin in the Repository
            Spin spin = new Spin()
            {
                IsWinning = spinItVM.Winner
            };
            //TODO: Update persistent data using dbcRepo.Spins.Add() and SaveChanges()
            repository.AddSpin(spin);

            return View("SpinIt", spinItVM);
        }

        /***
         * ListSpins Action
         **/
         [HttpGet]
         public IActionResult LuckList()
        {
            //TODO: Pass the View the Spins collection from the dbcRepo
            return View(repository.PlayerSpins);
        }

    }
}

