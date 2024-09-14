# SonmWikiRest

REST api Wiki Engine

## Requirements:
- postgres
- keycloak

## Basic Keycloak Setup

### Create a realm
### Create client for wiki backend
- Clients -> Create client -> Client ID: {your-backend-client-id}
  - Client authentication ✅
  - Authorization ✅
  - Standard and Direct access grants flows ✅ (you can also enable Implicit flow for testing purposes only)
  - Login setting (still not sure) (put * in Root URL, Home URL, Valid redirect URIs and Web origins  for testing)
### Create roles in wiki client
- Clients -> {your-backend-client-id} -> Client details -> Roles -> Create roles:
  - admin-role
  - editor-role
  - user-role
### Add audience mapper
- Clients -> {your-backend-client-id} -> Client details -> Client scopes
- Select {your-backend-client-id}-dedicated
- Add mapper -> By configuration -> Audience -> Included Client Audience -> select your client
### (Optional) Add mapper that replaces keycloak Id to username
  - Clients -> {your-backend-client-id} -> Client details -> Client scopes
  - Select {your-backend-client-id}-dedicated
  - Mappers -> Add mapper -> by configuration -> User property

        Name: Map username to sub (id)
        Property: username
        Token Claim Name: sub
        ClaimJsonType: String
        Add to Id token ✅
        Add to access token ✅
        Add to userinfo ✅
        Add to token introspection ✅
### Create client for wiki frontend
- Clients -> Create client -> Client ID: {your-frontend-client-id}
- Client authentication ❌
- Authorization ❌
- Standard and Direct access grants flows ✅
- Login setting (still not sure) (put * in Root URL, Home URL, Valid redirect URIs and Web origins  for testing)
### Configure frontend client
- Clients -> {your-frontend-client-id} -> Client details -> Client scopes
- Select {your-frontend-client-id}-dedicated
- Scope -> disable Full scope allowed
- Scope -> assign all roles that should be visible to that client