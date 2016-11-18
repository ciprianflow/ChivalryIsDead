 node ('linux'){
  stage 'Build and Test'
  env.PATH = "${tool 'Maven 3'}/bin:${env.PATH}"
  git url: "https://github.com/korineczek/ChivalryIsDead.git"
  sh 'mvn clean package'
 }
