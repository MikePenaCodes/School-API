using System;

class GfG
{
    static void merge(int[] arr, int l, int m, int r)
    {
        int n1 = m - l + 1;
        int n2 = r - m;

        int[] L = new int[n1];
        int[] R = new int[n2];
        int i, j;

        for (i = 0; i < n1; ++i)
        {
            L[i] = arr[l + i];
        }
        for (j = 0; j < n2; ++j)
        {
            R[j] = arr[m + 1 + j];
        }

        i = 0;
        j = 0;

        int k = l;
        while (i < n1 && j < n2)
        {
            if (L[i] <= R[j])
            {
                arr[k] = L[i];
                ++i;

            }
            else
            {
                arr[k] = R[j];
                ++j;
            }
            ++k;
        }
        while (i < n1)
        {
            arr[k] = L[i];
            ++i;
            ++k;
        }
        while (j < n2)
        {
            arr[k] = R[j];
            ++j;
            ++k;
        }




    }




    static void merge_sort(int[] arr, int l, int r)
    {
        if (l < r)
        {
            int m = l + (r - l) / 2;

            merge_sort(arr, l, m);
            merge_sort(arr, m + 1, r);

            merge(arr, l, m, r);
        }
    }


    static void printArray(int[] arr)
    {
        int n = arr.Length;
        for (int i = 0; i < n; ++i)
        {
            Console.Write(arr[i] + " ");
        }
        Console.ReadLine();
    }


    public static void Main()
    {
        int[] arr = { 5, 8, 4, 2, 7 };
        Console.WriteLine("Before.");
        printArray(arr);
        Console.WriteLine("After.");
        merge_sort(arr, 0, arr.Length - 1);
        printArray(arr);
        Console.ReadLine();

    }


}
