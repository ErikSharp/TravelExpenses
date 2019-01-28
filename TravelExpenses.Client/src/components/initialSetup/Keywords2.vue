<template>
  <v-card class="pa-3">
    <v-flex xs12 class="mb-3">
      <v-layout row align-center>
        <v-avatar size="55" class="mr-3 elevation-4">
          <v-icon large class="primary white--text">my_location</v-icon>
        </v-avatar>
        <h1>Keywords</h1>
      </v-layout>
    </v-flex>
    <p>The following is a pre-defined list of typical Keywords you might want to use.</p>
    <p>Refine the selection below to your liking:</p>
    <v-select :items="keywords" v-model="chosenKeywords" label="Keywords" chips solo multiple>
      <template slot="selection" slot-scope="data">
        <v-chip :selected="data.selected" close @input="removeKeyword(data.item)">
          <span>{{ data.item }}</span>
        </v-chip>
      </template>
      <template slot="item" slot-scope="data">
        <v-checkbox v-model="chosenKeywords" color="primary" :value="data.item" :label="data.item"/>
      </template>
    </v-select>
    <p>
      <strong>Note:</strong> You will be able to add additional Keywords by using the Setup menu within the application.
    </p>
    <v-flex xs12>
      <v-layout row justify-center>
        <v-btn :loading="busy" :disabled="!chosenKeywords.length" color="primary" @click="next">NEXT</v-btn>
      </v-layout>
    </v-flex>
  </v-card>
</template>

<script>
import clone from 'lodash/clone'

export default {
  created() {
    this.chosenKeywords = clone(this.keywords)
  },
  data() {
    return {
      chosenKeywords: []
    }
  },
  methods: {
    next() {
      this.$store
        .dispatch('Keyword/addKeywords', this.chosenKeywords)
        .then(() => {
          this.$store.dispatch('InitialSetup/nextWindow')
        })
    },
    removeKeyword(item) {
      this.chosenKeywords.splice(this.chosenKeywords.indexOf(item), 1)
      this.chosenKeywords = [...this.chosenKeywords]
    }
  },
  computed: {
    keywords() {
      return this.$store.state.Keyword.sampleKeywords
    },
    busy() {
      return this.$store.state.Keyword.addKeywordBusy
    }
  }
}
</script>

<style scoped>
</style>
