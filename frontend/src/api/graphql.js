import {
    ApolloClient,
    InMemoryCache,
    HttpLink,
  } from "@apollo/client";
  
  const client = new ApolloClient({
    link: new HttpLink({
      uri: "http://localhost:5011/graphql", // backend GraphQL endpoint
      headers: {
        "X-API-KEY": "simple-api-key",
      },
    }),
    cache: new InMemoryCache(),
  });
  
  export default client;
  