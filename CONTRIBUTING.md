# Contributing

When contributing to this repository, please first discuss the change you wish to make via issue,
email, or any other method with the owners of this repository before making a change. 

Please note we have a [code of conduct](./CODE_OF_CONDUCT.md), please follow it in all your interactions with the project.

## Pull Request Process

1. Ensure any additional install or build dependencies are removed before the end of the layer when doing a 
   build.
1. Update the README.md with details of changes to the interface, this includes new environment 
   variables, exposed ports, useful file locations and container parameters.
1. You may merge the Pull Request in once you have the sign-off of two other developers, or if you 
   do not have permission to do that, you may request the second reviewer to merge it for you.

## Conventions

### Trello 

This project uses [trello](#https://trello.com) for organization (it is currently private so only authorized members can join).
Format that is used for accepted trello logs is `KLRNS-<number>: <description>` (ex. `KLRNS-0019: Documentation - Getting Started`)

Since the `<number>` in `KLRNS-<number>` is custom generated an utility script `klrns_id_generate` has been written to save time for developers when deciding on the name. You can use it by copy pasting source code from the mentioned file into the browser `Console`.

### Commits

For clarity it is advised for commits to be in format `KLRNS-<number> | <commit-author> | <commit-description>` where (`KLRNS-<number>` should be equivalent to the adequate log item from [trello](#trello))