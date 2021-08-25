using System.Threading;
using System.Threading.Tasks;
using Application.Core;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Activities
{
    public class Create
    {
        public class Command : IRequest<Result<Activity>>
        {
            public Activity Activity { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Activity).SetValidator(new ActivityValidator());
            }
        }

        public class Handler : IRequestHandler<Command, Result<Activity>>
        {
            private readonly DataContext _context;
            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<Activity>> Handle(Command request, CancellationToken cancellationToken)
            {
                var createdActivity = _context.Activities.Add(request.Activity);

                var rowsAffectedInDatabase = await _context.SaveChangesAsync(cancellationToken) > 0;

                if(!rowsAffectedInDatabase) return Result<Activity>.Failure("Failed to create activity, please retry");

                var result = await _context.Activities.FindAsync(createdActivity.Entity.Id);

                return Result<Activity>.Success(result);
            }
        }
    }
}