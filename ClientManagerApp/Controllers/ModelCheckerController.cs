using ClientManagerApp.Models;
using ClientManagerApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace ClientManagerApp.Controllers
{
    public class ModelCheckerController : Controller
    {
        private readonly ModelCheckerService _modelCheckerService;

        public ModelCheckerController()
        {
            _modelCheckerService = new ModelCheckerService();
        }

        public IActionResult Index()
        {
            // Создаем состояния
            State login = new() { Name = "Login" };
            State initialization = new() { Name = "Initialization" };
            State mainTable = new() { Name = "MainTable" };
            State addClient = new() { Name = "AddClient" };
            State editClient = new() { Name = "EditClient" };
            State clientCard = new() { Name = "ClientCard" };

            // Создаем переходы
            login.Transitions = new List<Transition>
            {
                new Transition { From = login, To = initialization, Action = "Authorize" }
            };
            initialization.Transitions = new List<Transition>
            {
                new Transition { From = initialization, To = mainTable, Action = "InitializeRecords" }
            };
            mainTable.Transitions = new List<Transition>
            {
                new Transition { From = mainTable, To = addClient, Action = "AddClient" },
                new Transition { From = mainTable, To = editClient, Action = "EditClient" },
                new Transition { From = mainTable, To = clientCard, Action = "ViewClientCard" },
                new Transition { From = mainTable, To = mainTable, Action = "DeleteClient" }
            };
            addClient.Transitions = new List<Transition>
            {
                new Transition { From = addClient, To = mainTable, Action = "SaveClient" }
            };
            editClient.Transitions = new List<Transition>
            {
                new Transition { From = editClient, To = mainTable, Action = "SaveClientChanges" }
            };
            clientCard.Transitions = new List<Transition>
            {
                new Transition { From = clientCard, To = mainTable, Action = "BackToMainTable" }
            };

            // Проверяем свойства CTL
            var results = new List<string>
            {
                // EF-свойства
                $"Property EF(MainTable): {_modelCheckerService.CheckEF(login, mainTable)}",
                $"Property EF(AddClient): {_modelCheckerService.CheckEF(mainTable, addClient)}",
                $"Property EF(EditClient): {_modelCheckerService.CheckEF(mainTable, editClient)}",
                $"Property EF(ClientCard): {_modelCheckerService.CheckEF(mainTable, clientCard)}",

                // AG-свойства
                $"Property AG(MainTable -> EF(AddClient)): {_modelCheckerService.CheckAG(mainTable, state => _modelCheckerService.CheckEF(state, addClient))}",
                $"Property AG(MainTable -> EF(EditClient)): {_modelCheckerService.CheckAG(mainTable, state => _modelCheckerService.CheckEF(state, editClient))}",
                $"Property AG(ClientCard -> MainTable): {_modelCheckerService.CheckAG(clientCard, state => _modelCheckerService.CheckEF(state, mainTable))}",

                // AX-свойства
                $"Property AX(MainTable -> AddClient or EditClient): {_modelCheckerService.CheckAX(mainTable, state => _modelCheckerService.CheckEF(state, addClient))}",
                $"Property AX(ClientCard -> MainTable): {_modelCheckerService.CheckAX(clientCard, state => _modelCheckerService.CheckEF(state, addClient))}"
            };

            // Передаем результаты в представление
            return View(results);
        }
    }
}
