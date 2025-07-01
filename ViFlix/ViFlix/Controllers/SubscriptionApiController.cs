using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ViFlix.Core.Services.Subscription.SubscriptionPlanService;
using ViFlix.Core.Services.Subscription.UserSubscriptionService;
using ViFlix.Core.ViewModels.SubscriptionViewModels;

namespace ViFlix.Controllers
{
    #region SubscriptionPlan
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionPlanController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IsubscriptionPlanServices _subsPlan;

        public SubscriptionPlanController(IMapper mapper , IsubscriptionPlanServices subsPlan)
        {
            _mapper = mapper;
            _subsPlan = subsPlan;
        }

        public List<SuscriptionPlanViewModel> plansList { get; set; }

        [HttpGet]
        public List<SuscriptionPlanViewModel> GetAllPlans()
        {
            plansList = _subsPlan.GetAllPlans().ToList();
            return plansList;
        }

        [HttpGet("{id}")]
        public ActionResult<SuscriptionPlanViewModel> GetPlansById(long id)
        {
            var plan = _subsPlan.GetPlanById(id);
            if (plan == null)
                return NotFound(plan);
            else
                return Ok(plan);
        }

        [HttpPost]
        public long AddPlansFromApi(long id)
        {
            return id;
        }

        [HttpPost("AddSubsPlans")]
        public async Task<long> AddSubsPlans([FromForm] SuscriptionPlanViewModel plan)
        {
            return await _subsPlan.AddPlan(plan);
        }

        [HttpPut("EditPlans")]
        public async Task<IActionResult> EditPlans([FromForm] SuscriptionPlanViewModel plan, long id)
        {
            var existPlan = _subsPlan.GetPlanById(id);
            if (existPlan == null)
            {
                return NotFound("Plan not found!");
            }

            existPlan.PlanName = plan.PlanName;
            existPlan.Price = plan.Price;
            existPlan.DurationDays = plan.DurationDays;
            existPlan.Description = plan.Description;
            existPlan.IsActive = plan.IsActive;


            await _subsPlan.EditPlans(existPlan);

            return Ok();
        }

        [HttpDelete("DeletePlans")]
        public async Task<IActionResult> DeletePlans(long id)
        {
            var plan = _subsPlan.GetPlanById(id);

            if (plan == null)
            {
                return NotFound(new { message = "Not found !" });
            }

            await _subsPlan.DeletePlans(id);

            return Ok();
        }

        [HttpGet("GetactivePlans")]
        public List<SuscriptionPlanViewModel> GetactivePlans()
        {
            var plans = _subsPlan.GetActivePlans().ToList();
            return plans;
        }

        [HttpGet("CheckExistPlans/{id}")]
        public IActionResult CheckExistPlans(long id)
        {
            var plan = _subsPlan.CheckIfPlanExists(id);

            if (plan)
            {
                return Ok(new { status = true, message = "Plan exists ✅" });
            }
            else
            {
                return NotFound(new { status = false, message = "Plan not found ❌" });
            }
        }
    }
    #endregion

    #region UserSubscription
   
    [Route("api/[controller]")]
    [ApiController]
    public class UserSubscribTionController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserSubscriptionServices _UserSub;

        public UserSubscribTionController(IMapper mapper , IUserSubscriptionServices UserSub)
        {
            _mapper = mapper;
            _UserSub = UserSub;
        }

        public List<UserSubscriptionViewModel> UserSubList { get; set; }

        [HttpGet]
        public List<UserSubscriptionViewModel> GetAllSubscriber()
        {
            UserSubList = _UserSub.GetAllsubscriber().ToList();
            return UserSubList;
        }

        [HttpGet("{id}")]
        public ActionResult<UserSubscriptionViewModel> GetSubscriberById(long? id)
        {
            var subs = _UserSub.GetSubscriberById(id);
            if (subs == null)
                return NotFound(subs);
            else
                return Ok(subs);
        }

        [HttpPost]
        public long AddSubscriberFromApi(long id)
        {
            return id;
        }

        [HttpPost("AddSubsPlans")]
        public async Task<long> AddSubscribers([FromForm] UserSubscriptionViewModel subs)
        {
            return await _UserSub.AddSubscriber(subs);
        }


        [HttpPut("EditSubscriber")]
        public async Task<IActionResult> EditSubscriber([FromForm] UserSubscriptionViewModel sub, long id)
        {
            var existsub= _UserSub.GetSubscriberById(id);
            if (existsub == null)
            {
                return NotFound("Subscriber not found!");
            }

            existsub.StartDate = sub.StartDate;
            existsub.EndDate = sub.EndDate;
            existsub.IsActive = sub.IsActive;
            existsub.UserId = sub.UserId;
            existsub.PlanId = sub.PlanId;


            await _UserSub.EditSubscriber(existsub);

            return Ok();
        }

        [HttpDelete("DeleteSubscriber")]
        public async Task<IActionResult> DeleteSubscriber(long id)
        {
            var subs = _UserSub.GetSubscriberById(id);

            if (subs == null)
            {
                return NotFound(new { message = "Not found !" });
            }

            await _UserSub.DeleteSubscriber(id);

            return Ok();
        }

        [HttpGet("GetSubscriberByUserId")]
        public ActionResult<UserSubscriptionViewModel> GetSubscriberByUserId(long userId)
        {
            var subs = _UserSub.GetSubscriberByUserIdAsync(userId);

            if (subs == null)
            {
                return NotFound("subscriber is not Found");
            }

            return Ok();
        }


        [HttpGet("IsUserSubscribed/{userId}")]
        public IActionResult IsUserSubscribed(long userId)
        {
            var subs = _UserSub.IsUserSubscribed(userId);

            if (subs)
            {
                return Ok(new { status = true, message = "User has an active subscription ✅" });
            }
            else
            {
                return NotFound(new { status = false, message = "User does NOT have an active subscription ❌" });
            }
        }
    }
        #endregion
}
