import camelCase from 'lodash.camelcase';

export const keysToCamel = <T>(obj: any): T => {
  if (Array.isArray(obj)) {
    return obj.map(keysToCamel) as unknown as T;
  } else if (obj !== null && typeof obj === 'object') {
    return Object.entries(obj).reduce((acc, [key, value]) => {
      const newKey = camelCase(key);
      acc[newKey] = keysToCamel(value);
      return acc;
    }, {} as Record<string, any>) as T;
  }
  return obj;
};
