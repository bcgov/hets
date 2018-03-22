export PROJECT_NAMESPACE=${PROJECT_NAMESPACE:-tran-hets}
export GIT_URI=${GIT_URI:-"https://github.com/bcgov/hets.git"}
export GIT_REF=${GIT_REF:-"master"}

# The templates that should not have their GIT referances(uri and ref) over-ridden
# Templates NOT in this list will have they GIT referances over-ridden
# with the values of GIT_URI and GIT_REF
export skip_git_overrides=${skip_git_overrides:-"schema-spy-build.json dotnet-20-runtime-centos7-build.json dotnet-20-centos7-build.json"}

# The project components
# - They are all contained under the main OpenShift folder.
export components=${components:-"."}

# The builds to be triggered after buildconfigs created (ones that are not auto-triggered)
export builds=${builds:-""}

# The images to be tagged after build
export images=${images:-"server pdf backup frontend schema-spy weasyprint"}

# The routes for the project
export routes=${routes:-"server schema-spy pdf frontend"}
