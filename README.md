# TransitiveDependenciesApp

This app is based on the directions for the [Snyk Exercise](https://github.com/snyk/jobs/tree/master/exercises/npm-registry).

## Task

1. Update the `/package` endpoint, so that it returns all of the transitive
   dependencies for a package, not only the first-order dependencies

2. Present these dependencies in a tree view that can be viewed from a Web Browser (you can use any technologies you find suitable)

## Instructions

1. Unzip the [file](https://drive.google.com/file/d/1pI2yc9HlUtl3S0AATZDLt7B4nTyeSuIw/view?usp=drive_web), and run the exe file.
2. The web server will run on localhost:5000.
3. Go to the URL http://localhost:5000/package/react/16.13.0 for example and see the result.

## Implementation 
This is an ASP.NET app, written in C# on the server-side and in React, Javascript on the client-side.

On the server-side there's the `Get` method `/api/package/{name}/{version}` that is responsible for the retrieval 
of the data from the [Npm package API](https://registry.npmjs.org/). 
Implemented a recursive function with memoization.

On the client-side used javascript, [React](https://reactjs.org/) and [Material-UI](https://material-ui.com/) to 
represent the tree structure using the [TreeView React Component](https://material-ui.com/components/tree-view/).
