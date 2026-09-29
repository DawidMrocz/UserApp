import { Ref, ref, UnwrapRef, watch } from 'vue';

export default function <T>(
  getter: () => T,
  setter: (val: T) => unknown
): Ref<T> {
  const propRef = ref<T>(getter());

  watch(getter, (val) => {
    if (val != propRef.value) {
      propRef.value = val as UnwrapRef<T>;
    }
  });

  watch(propRef, (val) => {
    if (val != getter()) {
      setter(val as T);
    }
  });

  return propRef as Ref<T>;
}
