﻿using System.IO;
using System.Threading.Tasks;
using Common.Utils;
using Google.Cloud.Dialogflow.V2;
using Google.Protobuf;
using MediatR;
using MessengerBotAPI.ApiContract;
using MessengerBotAPI.Application.Basket.GetBasketInformation;
using MessengerBotAPI.Application.Basket.RemoveBasketItem;
using MessengerBotAPI.Application.ChangeLanguage;
using MessengerBotAPI.Application.GetCategoryMenu;
using MessengerBotAPI.Application.GetDeliveryStatus;
using MessengerBotAPI.Application.GetFoodDetails;
using MessengerBotAPI.Application.Order.AddToCart;
using MessengerBotAPI.Application.Order.LastOrderRepeat;
using MessengerBotAPI.Application.Order.PlaceOrder;
using MessengerBotAPI.Application.Restaurant.GetMenu;
using MessengerBotAPI.Application.Restaurant.GetRestaurantList;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MessengerBotAPI.Controllers
{
    [Route("api/bot")]
    [ApiController]
    public class MessageController : BaseController
    {
        private readonly SessionsClient _client;
        private const string ProjectId = "food-delivery-umawew";


        public MessageController(IMediator mediator) : base(mediator)
        {
            _client  = SessionsClient.Create();
            ;
        }

        [HttpPost("detect-intent")]
        public async Task<IActionResult> DetectIntent(DetectTextIntentRequest request)
        {
            var response = await _client.DetectIntentAsync(
                session: SessionName.FromProjectSession(ProjectId, request.SessionId),
                queryInput: new QueryInput()
                {
                    Text = new TextInput()
                    {
                        Text = request.Text,
                        LanguageCode = "en"
                    }
                }
            );

            return await HandleIntent(response.QueryResult, request);
        }

        private async Task<IActionResult> HandleIntent(QueryResult queryResult, DetectTextIntentRequest request)
        {
            switch (queryResult.Intent.DisplayName)
            {
                //command
                case UserIntents.RemoveBasketItem:
                    return await HandleActionResultFor(new RemoveBasketItemCommand(queryResult,request));

                case UserIntents.OrderFood:
                    return await HandleActionResultFor(new AddFoodToCartCommand(queryResult,request));

                case UserIntents.PlaceOrder:
                    return await HandleActionResultFor(new PlaceOrderCommand(queryResult,request));
                
                case UserIntents.RepeatLastOrder:
                    return await HandleActionResultFor(new RepeatLastOrderCommand(queryResult,request));
                
                case UserIntents.ChangeLanguage:
                    return await HandleActionResultFor(new ChangeLanguageCommand(queryResult,request));


                //query
                case UserIntents.CheckBasket:
                    return await HandleQueryResultFor(new GetBasketInformationQuery(queryResult,request));

                case UserIntents.GetCategoryMenu:
                    return await HandleQueryResultFor(new GetCategoryMenuQuery(queryResult,request));

                case UserIntents.GetDeliveryStatus:
                    return await HandleQueryResultFor(new GetDeliveryStatusQuery(queryResult,request));

                case UserIntents.GetFoodDetails:
                    return await HandleQueryResultFor(new GetFoodDetailQuery(queryResult,request));

                case UserIntents.GetRestaurants:
                case UserIntents.GetMoreRestaurants:
                    return await HandleQueryResultFor(new GetRestaurantListQuery(queryResult,request));
                case UserIntents.GetRestaurantMenu:
                    return await HandleQueryResultFor(new GetRestaurantMenuQuery(queryResult,request));
                
                default:
                    return Ok(Envelope.Ok(queryResult.FulfillmentText));
                
            }
        }
        
        
        /**
         * this class should not be accessed from outside
         */
        private class UserIntents
        {
            public const string RemoveBasketItem = "delete.basket_item";
            public const string CheckBasket = "get.cart_status";
            public const string GetCategoryMenu = "get.category_menu";
            public const string GetDeliveryStatus = "get.delivery_status";
            public const string GetFoodDetails = "get.food_detail";
            public const string GetRestaurants = "get.restaurant_list";
            public const string GetMoreRestaurants = "browse.restaurant - more";
            public const string GetRestaurantMenu = "get.restaurant_menu";
            public const string OrderFood = "order.snack";
            public const string ProvideAddress = "post.delivery_address";
            public const string PlaceOrder = "post.order";
            public const string RepeatLastOrder = "post.repeat_last";
            public const string ChangeLanguage = "post.change_language";
        }
    }

    
}