using ClientManagerApp.Models;
using System.Collections.Generic;

namespace ClientManagerApp.Services
{
    public class ModelCheckerService
    {
        // Проверка EF: Существует ли путь к целевому состоянию
        public bool CheckEF(State initialState, State targetState)
        {
            // Инициализация множества для отслеживания посещенных состояний
            var visited = new HashSet<State>();

            // Инициализация стека для обхода в глубину (DFS)
            var stack = new Stack<State>();

            // Добавляем начальное состояние в стек
            stack.Push(initialState);

            // Пока в стеке есть состояния для обработки
            while (stack.Count > 0)
            {
                // Извлекаем текущее состояние из стека
                var currentState = stack.Pop();

                // Проверяем, совпадает ли текущее состояние с целевым
                if (currentState == targetState)
                    return true; // Если найдено, возвращаем true

                // Проверяем, было ли состояние уже посещено
                if (!visited.Contains(currentState))
                {
                    // Помечаем состояние как посещенное
                    visited.Add(currentState);

                    // Проверяем, есть ли переходы из текущего состояния
                    if (currentState.Transitions != null)
                    {
                        // Для каждого перехода из текущего состояния
                        foreach (var transition in currentState.Transitions)
                        {
                            // Добавляем целевое состояние перехода в стек
                            stack.Push(transition.To);
                        }
                    }
                }
            }

            // Если стек опустел и целевое состояние не было найдено, возвращаем false
            return false;
        }



        public bool CheckAX(State initialState, Func<State, bool> predicate)
        {
            // Проверяем все состояния, достижимые через один переход
            if (initialState.Transitions != null)
            {
                foreach (var transition in initialState.Transitions)
                {
                    // Если хотя бы одно состояние не удовлетворяет предикату, возвращаем false
                    if (!predicate(transition.To))
                    {
                        Console.WriteLine($"AX failed for state: {transition.To.Name}");
                        return false;
                    }
                }
            }

            // Если все состояния удовлетворяют предикату, возвращаем true
            return true;
        }


        public bool CheckAG(State initialState, Func<State, bool> predicate)
        {
            var visited = new HashSet<State>();
            var stack = new Stack<State>();
            stack.Push(initialState);

            while (stack.Count > 0)
            {
                var currentState = stack.Pop();

                // Логирование текущего состояния
                Console.WriteLine($"Checking state: {currentState.Name}");

                // Если текущее состояние не удовлетворяет предикату, возвращаем false
                if (!predicate(currentState))
                {
                    Console.WriteLine($"Predicate failed for state: {currentState.Name}");
                    return false;
                }

                if (!visited.Contains(currentState))
                {
                    visited.Add(currentState);

                    if (currentState.Transitions != null)
                    {
                        foreach (var transition in currentState.Transitions)
                        {
                            // Переход на следующее состояние
                            stack.Push(transition.To);
                        }
                    }
                }
            }

            return true; // Если все состояния удовлетворяют предикату
        }





    }
}
