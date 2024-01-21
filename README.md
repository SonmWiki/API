# SonmWikiRest

REST api Wiki Engine

## Requirements:
- postgres
- keycloak

## Basic Keycloak Setup

Create Realm

Create Client: wiki-api

Client auth [x]

Auth [x]

Standard flow[x], Direct Access grants [x]

Valid redirect URIs ?
Web origins?

Roles:

Create roles:
- admin-role
- editor-role
- user-role 

Create mapper:
- Mappers: username_to_sub(id)_mapping
- Mapper type: User Property
- Property: username
- Token Claim Name: sub
- ClaimJsonType: String
- Add to Id token [x]
- Add to access token [x]
- Add to userinfo [x]
- Add to token introspection [x]

Add this mapper to client scopes

Client scopes: new username_to_sub(id)_mapping Type: default

keycloak.json ???