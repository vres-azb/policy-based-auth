# Policy-based Authorization Demo

![GitHub Cloned](https://img.shields.io/badge/dynamic/json?color=success&label=cloned&query=count&url=https://gist.githubusercontent.com/vres-azb/ee2bf887d1d26d36a0919e16416516d5/raw/clone.json&logo=github)


## Intro
This demo exemplified the separation of concerns of `authentication` and `authorization`. The same way you centralize the authentication to an `Identity Provider`, you should do the same for an `Authorization Provider`.

The scenario presented is on a common use case for iVal, as defined in the `Role Based Matrix.xls`.


### Identity + Permissions = Authorization

![Identity + Permissions = Authorization](./assets/sc01.png)

### AuthZ Policy-based service

![AuthZ Policy-based service](./assets/sc02.png)