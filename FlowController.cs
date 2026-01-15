using System;
using System.Collections.Generic;

class FlowController
{
    static void Main()
    {
        //Start på Flowchart 
        Console.WriteLine("Start");
        Console.WriteLine("Admin logger ind");
        
        //Admin logger ind og indsætter modtaget ordre 
        bool adminLoggedIn = true;
        
        //"Database" af ordrer - Hver ordre er en liste af komponenter
        List<List<string>> orderDatabase = new List<List<string>>();
        
        //Første ordre 
        AddOrderWithQty(orderDatabase);
        
        Console.WriteLine("Admin kigger ordre databasen");

       
        
        //Er der flere ordre?
        while (adminLoggedIn)
        {
            //Hvis der ikke er flere ordre end den ene ordre, så logger Admin ud. 
            if (orderDatabase.Count == 0)
            {
                Console.WriteLine("Flere ordre? -> No");
                Console.WriteLine("Admin logger ud");
                break;
            }
            
            // Hvis der er flere ordre, vil man skulle indtaste disse
            Console.WriteLine("\nFlere ordre? -> Yes");
            Console.WriteLine("Load order");
            
            //Load order 
            List<string> currentOrder = orderDatabase[0];
            orderDatabase.RemoveAt(0);
            
            //Hvis ordren har flere produkter 
            if (currentOrder.Count == 0)
            {
                Console.WriteLine("Ordren indeholder flere produkter? -> No");
            }
            else
            {
                Console.WriteLine("Ordren indeholder flere produkter? -> Yes");
                
                bool pickOk = AskYesNo("Afhenting succesful for hele ordren? (y/n): ");
                if (!pickOk)
                {
                    Console.WriteLine("Gå til start position");
                    Console.WriteLine("Fejl besked (Afhentning fejlede)");
                    continue; 
                }
                
                bool placeOk = AskYesNo("Successful placering for hele ordren? (y/n): ");
                if (!placeOk)
                {
                    Console.WriteLine("Failure message (placement failed)");
                    continue;
                }
                
            }
            while (currentOrder.Count > 0)
            {
                Console.WriteLine("Ordre indeholder flere produkter -> Yes");
                
                //Identificer komponent fra order 
                string component = currentOrder[0];
                currentOrder.RemoveAt(0);
                
                Console.WriteLine("Identificer komponent fra ordre: ");
                Console.WriteLine("Hent komponent");
                
                Console.WriteLine("Flyt komponent til pakke position");
                Console.WriteLine("Placer komponent i pakke");
                
                Console.WriteLine("Log component as successful");
                Console.WriteLine("Decrease component quantity in overview");
            }

            Console.WriteLine("Order contains more products? -> No");
            Console.WriteLine("Admin confirms order completion");
            
            //Ny ordre?
            bool enterNewOrder = AskYesNo("\nDo you want to enter another order? (y/n): ");
            if (enterNewOrder)
            {
                AddOrderWithQty(orderDatabase);
            }
            else
            {
                Console.WriteLine("Admin logs out");
                adminLoggedIn = false;
            }
        }

        Console.WriteLine("End");
    }

    static void AddOrderWithQty(List<List<string>> orderDatabase)
    {
        List<string> order = new List<string>();

        Console.WriteLine("\n Admin indtaster ordre");
        Console.WriteLine("Skriv komponentnavn og antal. Skriv 'done' når du er færdig");

        while (true)
        {
            Console.Write("Komponent eller 'done':");
            string component = (Console.ReadLine() ?? "").Trim();
            
            if (component.ToLower() == "done")
                break;

            Console.Write("Antal: ");
            string qtyText = (Console.ReadLine() ?? "").Trim();

            if (!int.TryParse(qtyText, out int qty) || qty <= 0)
            {
                Console.WriteLine("Antal skal være et tal > 0");
                continue;
            }

            for (int i = 0; i < qty; i++)
                order.Add(component);
        }

        if (order.Count == 0)
        {
            Console.WriteLine("Ordre blev ikke gemt (ingen komponenter).");
            return;
        }

        orderDatabase.Add(order);
        Console.WriteLine("Ordre gemt! Total komponenter i ordren: " + order.Count);
    }
    static bool AskYesNo(string question)
    {
        while (true)
        {
            Console.Write(question);
            string input = (Console.ReadLine() ?? "").Trim().ToLower();

            if (input == "y") return true;
            if (input == "n") return false;

            Console.WriteLine("Skriv y eller n.");
            }
        }
    }