pipeline {
  agent { label '!master' }

  environment {
    DOCKERHUB_ID= 'ssivazhalezau'
    DOCKERHUB_CR= 'a19159bb-082d-4be5-90b9-ca05a582eb06'
  }

  stages {
    stage ('Checkout source') {
      steps {
        cleanWs()
        checkout scm
      }
    }

    stage ('Build images') {
      steps {
        script {
          dir("src/Exadel.CrazyPrice") {
            identity   = docker.build("${DOCKERHUB_ID}/identity:${currentBuild.number}", '-f Dockerfile.identity .')
            webapi     = docker.build("${DOCKERHUB_ID}/webapi:${currentBuild.number}", '-f Dockerfile.webapi .')
            mailsender = docker.build("${DOCKERHUB_ID}/mailsender:${currentBuild.number}", '-f Dockerfile.mailsender .')
          }
        }
      }
    }

    stage ('Push images') {
      steps {
        script {
          docker.withRegistry('https://registry.hub.docker.com', "${DOCKERHUB_CR}") {
            identity.push("${currentBuild.number}")
            identity.push('latest')
            webapi.push("${currentBuild.number}")
            webapi.push('latest')
            mailsender.push("${currentBuild.number}")
            mailsender.push('latest')
          }
        }
      }
    }
  }
}