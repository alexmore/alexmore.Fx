#region License
/******************************************************************************
Copyright (c) 2016 Alexandr Mordvinov, alexandr.a.mordvinov@gmail.com

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
******************************************************************************/
#endregion

using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace alexmore.Fx
{
    public static class StreamUtils
    {
        public static string ReadString(this Stream stream, Encoding encoding = null)
        {
            if (encoding.IsNull()) encoding = Encoding.UTF8;

            using (var reader = new StreamReader(stream, encoding))
            {
                return reader.ReadToEnd();
            }
        }

        public static async Task<string> ReadStringAsync(this Stream stream, Encoding encoding = null)
        {
            if (encoding.IsNull()) encoding = Encoding.UTF8;

            using (var reader = new StreamReader(stream, encoding))
            {
                return await reader.ReadToEndAsync().ConfigureAwait(false);
            }
        }

        public static IEnumerable<string> ReadAllLines(this Stream stream, Encoding encoding = null)
        {
            if (encoding.IsNull()) encoding = Encoding.UTF8;

            using (var reader = new StreamReader(stream, encoding))
            {
                while (!reader.EndOfStream)
                {
                    yield return reader.ReadLine();
                }
            }
        }

        public static async Task<IEnumerable<string>> ReadAllLinesAsync(this Stream stream, Encoding encoding = null)
        {
            if (encoding.IsNull()) encoding = Encoding.UTF8;

            using (var reader = new StreamReader(stream, encoding))
            {
                var result = new List<string>();
                while (!reader.EndOfStream)
                    result.Add(await reader.ReadLineAsync());

                return result;
            }
        }
    }
}
