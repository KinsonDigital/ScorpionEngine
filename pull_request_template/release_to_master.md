This <span style="color:whitesmoke;font-weight:bold">PR</span> merges everything related to release <span style="color:darkorange;font-weight:bold">v[version]</span> into the <span style="color:whitesmoke;/font-weight:bold">master</span> branch

---

<span style="color:gold;font-weight:bold;font-size:14pt">!!WARNING!!</span> - Completing this PR will automatically release the software to production

<span style="color:whitesmoke;font-weight:bold">NOTE:</span> Replace text in <span style="color:darkorange;font-weight:bold">dark orange</span> with appropriate values

## <span style="color:mediumseagreen;font-weight:bold">PR To Do</span>

1. [ ] Verify that the name of the current release branch is correct
    * <span style="color:whitesmoke;font-weight:bold">NOTE:</span> Use [<span style="color:dodgerblue;font-weight:bold">semantic versioning</span>](https://semver.org/)
	* <span style="color:whitesmoke;font-weight:bold">Syntax:</span> release/v[version]
	* <span style="color:whitesmoke;font-weight:bold">Example:</span> release/v1.2.3
2. [ ] Verify the PR title:
   * <span style="color:whitesmoke;font-weight:bold">Example:</span> Release v1.2.3 - Merge Into Master
3. [ ] Verify that all related work item cards have been linked/added to the PR
4. [ ] Verify that the entire solution <span style="color:whitesmoke;font-weight:bold">builds</span> locally using <span style="color:mediumpurple;font-weight:bold;font-weight:bold">Visual Studio</span>
5. [ ] Verify that all <span style="color:whitesmoke;font-weight:bold">unit tests</span> pass locally using <span style="color:mediumpurple;font-weight:bold;font-weight:bold">Visual Studio</span>
6. [ ] Check the code to make sure that it follows coding standards
   * If any coding standard violations exist, create cards in **Azure DevOps** to fix these issues.
7. [ ] Update the version number on the release branch
8. [ ] Update the release notes on the release branch
	* <span style="color:whitesmoke;font-weight:bold">NOTE:</span> Make sure to browse through the card info and GIT commits to collect information for the release notes
9. [ ] Verify that the code changes have been reviewed and checked off for the PR

---

## <span style="color:mediumseagreen;font-weight:bold">After PR Completion To Do</span>

1. Verify the following after PR completion:
   *  Verify that the build has succeeded
   *  Verify that the release has succeeded
      * This means verifying that the nuget package or application has been published
2. Create a tag with the proper naming convention
   * <span style="color:whitesmoke;font-weight:bold">NOTE:</span> Create the tag <span style="color:whitesmoke;font-weight:bold">on</span> the <span style="color:whitesmoke;font-weight:bold">master</span> branch where the <span style="color:whitesmoke;font-weight:bold">release</span> branch has been merged into <span style="color:whitesmoke;font-weight:bold">master</span>
   * <span style="color:whitesmoke;font-weight:bold">Syntax:</span>: v[version]
   * <span style="color:whitesmoke;font-weight:bold">Example:</span>  v1.2.3
3. Update the **Version** field for all related cards for the release.
