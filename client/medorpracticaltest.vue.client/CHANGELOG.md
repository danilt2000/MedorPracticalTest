This file explains how Visual Studio created the project.

The following tools were used to generate this project:
- create-vite

The following steps were used to generate this project:
- Create vue project with create-vite: `npm init --yes vue@latest medorpracticaltest.vue.client -- --eslint `.
- Create project file (`medorpracticaltest.vue.client.esproj`).
- Create `launch.json` to enable debugging.
- Create `nuget.config` to specify location of the JavaScript Project System SDK (which is used in the first line in `medorpracticaltest.vue.client.esproj`).
- Add project to solution.
- Write this file.
