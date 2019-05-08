export function firstLetterUpper(inputString) {
  let result = inputString.replace(/([A-Z])/g, ' $1')
  return result.charAt(0).toUpperCase() + result.slice(1)
}

export function toLocaleStringWithEndingZero(numValue) {
  numValue += 0 //used to remove the negative zero
  let result = numValue.toLocaleString()

  if (result.substring(result.length - 2, result.length - 1) === '.') {
    result += '0'
  }

  return result
}
