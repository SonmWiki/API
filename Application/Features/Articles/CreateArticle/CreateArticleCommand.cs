﻿using ErrorOr;
using MediatR;

namespace Application.Features.Articles.CreateArticle;

public record CreateArticleCommand(
    string Title,
    string Content,
    string AuthorsNote,
    List<string> CategoryIds
) : IRequest<ErrorOr<CreateArticleResponse>>;