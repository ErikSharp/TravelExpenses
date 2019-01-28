<template>
  <div>
    <h2 class="white--text">Add additional keyword</h2>
    <v-text-field
      v-model.trim="keyword"
      :error-messages="keywordErrors"
      label="Enter keyword name"
      box
      background-color="white"
      color="primary"
      @input="$v.keyword.$touch()"
      @blur="$v.keyword.$touch()"
    ></v-text-field>
    <v-flex xs8 offset-xs2>
      <v-layout row justify-space-around>
        <v-btn dark color="primary" :disabled="$v.$invalid" :loading="busy" @click="add">Add</v-btn>
        <v-btn dark color="primary" @click="cancel">Cancel</v-btn>
      </v-layout>
    </v-flex>
  </div>
</template>

<script>
import { required, minLength, maxLength } from 'vuelidate/lib/validators'

const keywordMustBeUnique = (value, vm) => {
  let itemsLowered = vm.items.map(i => i.keywordName.toLowerCase())
  return itemsLowered.indexOf(value.toLowerCase()) < 0
}

export default {
  validations() {
    const result = {
      keyword: {
        required,
        minLength: minLength(3),
        maxLength: maxLength(255),
        keywordMustBeUnique
      }
    }

    return result
  },
  data() {
    return {
      keyword: ''
    }
  },
  methods: {
    cancel() {
      this.$emit('cancel')
    },
    add() {
      this.$store.dispatch('Keyword/addKeywords', [this.keyword])
    }
  },
  computed: {
    keywordErrors() {
      const errors = []

      if (!this.$v.keyword.$dirty) return errors

      !this.$v.keyword.maxLength &&
        errors.push(
          `The keyword can be a maximum of ${
            this.$v.keyword.$params.maxLength.max
          } characters`
        )
      !this.$v.keyword.minLength &&
        errors.push(
          `The keyword must be a minimum of ${
            this.$v.keyword.$params.minLength.min
          } characters`
        )
      !this.$v.keyword.keywordMustBeUnique &&
        errors.push('The keyword must be unique')

      !this.$v.keyword.required && errors.push('A keyword is required')
      return errors
    },
    items() {
      return this.$store.state.Keyword.keywords
    },
    busy() {
      return this.$store.state.Keyword.addKeywordBusy
    }
  },
  watch: {
    items() {
      this.keyword = ''
      this.$v.$reset()
    }
  }
}
</script>

<style scoped>
</style>
