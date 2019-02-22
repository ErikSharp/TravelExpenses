export function firstLetterUpper(inputString) {
  let result = inputString.replace(/([A-Z])/g, ' $1')
  return result.charAt(0).toUpperCase() + result.slice(1)
}

export function toLocaleStringWithEndingZero(numValue) {
  let result = numValue.toLocaleString()

  if (result.substring(result.length - 2, result.length - 1) === '.') {
    result += '0'
  }

  return result
}
